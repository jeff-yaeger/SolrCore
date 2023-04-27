namespace SolrCore.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Connection;
    using EntityDefaults;
    using EntityModels;
    using EntitySetter;
    using Models;
    using Query;
    using QueryBuilder;
    using Serializer;

    public class SolrRepository<TKey, T> : ISolrRepository<TKey, T> where T : class, IEntity<TKey>, ISolrId where TKey : IEquatable<TKey>
    {
        private static string _coreName;
        private readonly IEntitySetter _entitySetter;
        private readonly ISolrSerializer _serializer;
        private readonly ISolrConnection _solrConnection;

        public SolrRepository(ISolrConnection solrConnection,
            ISolrSerializer serializer,
            IEntitySetter entitySetter)
        {
            _solrConnection = solrConnection;
            _serializer = serializer;
            _entitySetter = entitySetter;
        }

        public async Task<bool> AddAsync(T item)
        {
            _entitySetter.Set<TKey>(new EntityTransaction { Entity = item, State = TransactionState.Add });

            await ConfirmIdNotInUse(item);

            return await _solrConnection.PostAsync(_serializer.Serialize(new[] { item }), _coreName);
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> items)
        {
            _entitySetter.SetRange<TKey>(items.Select(entity =>
                new EntityTransaction
                {
                    Entity = entity,
                    State = TransactionState.Add
                }));

            await ConfirmIdsNotInUse(items);

            return await _solrConnection.PostAsync(_serializer.Serialize(items), _coreName);
        }

        public async Task<bool> UpdateAsync<TUpdate>(TUpdate item) where TUpdate : SolrEntity<TUpdate>, IEntity<TKey>
        {
            _entitySetter.Set<TKey>(new EntityTransaction { Entity = item, State = TransactionState.Update });
            return await _solrConnection.PostAsync(_serializer.Serialize(new[] { item }), _coreName);
        }

        public async Task<bool> UpdateRangeAsync<TUpdate>(IEnumerable<TUpdate> items) where TUpdate : SolrEntity<TUpdate>, IEntity<TKey>
        {
            _entitySetter.SetRange<TKey>(items.Select(entity =>
                new EntityTransaction
                {
                    Entity = entity,
                    State = TransactionState.Update
                }));

            return await _solrConnection.PostAsync(_serializer.Serialize(items), _coreName);
        }

        public async Task<SolrResponse<T>> GetAsync(IQuery query)
        {
            var rendered = query.Render(new Builder(SolrEntity<T>.Translations, SolrEntity<T>.DefaultQueries));
            var responseContent = await _solrConnection.GetAsync(rendered, _coreName);
            return _serializer.Deserialize<SolrResponse<T>>(responseContent);
        }

        public async Task<bool> DeleteAllAsync<TDelete>() where TDelete : SolrEntity<TDelete>, IEntity<TKey>
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TDelete)) || typeof(IOnDelete<TKey>).IsAssignableFrom(typeof(TDelete)))
            {
                var queryBuilder = new QueryBuilder(
                    new Q(new AllQuery()),
                    new FL(new Fields("id")));

                var rendered = queryBuilder.Render(new Builder(SolrEntity<TDelete>.Translations, SolrEntity<TDelete>.DefaultQueries));
                var responseContent = await _solrConnection.GetAsync(rendered, _coreName);
                var response = _serializer.Deserialize<SolrResponse<TDelete>>(responseContent);

                _entitySetter.SetRange<TKey>(response.Response.Docs.Select(entity =>
                    new EntityTransaction
                    {
                        Entity = entity,
                        State = TransactionState.Delete
                    }));

                return await _solrConnection.PostAsync(_serializer.Serialize(response.Response.Docs), _coreName);
            }

            var serialize = GetDeleteQuery("*:*");
            return await _solrConnection.PostAsync(serialize, _coreName);
        }

        public async Task<bool> DeleteAsync<TDelete>(TKey id) where TDelete : class, IEntity<TKey>, new()
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TDelete)) || typeof(IOnDelete<TKey>).IsAssignableFrom(typeof(TDelete)))
            {
                var delete = new TDelete
                {
                    Id = id
                };

                _entitySetter.Set<TKey>(new EntityTransaction { Entity = delete, State = TransactionState.Delete });
                return await _solrConnection.PostAsync(_serializer.Serialize(new[] { delete }), _coreName);
            }

            var serialize = GetDeleteQuery(new ByField("id", id).Render(new Builder(SolrEntity<T>.Translations, SolrEntity<T>.DefaultQueries)));
            return await _solrConnection.PostAsync(serialize, _coreName);
        }

        public async Task<bool> DeleteAsync<TDelete>(Query query) where TDelete : SolrEntity<TDelete>, IEntity<TKey>
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TDelete)) || typeof(IOnDelete<TKey>).IsAssignableFrom(typeof(TDelete)))
            {
                var queryBuilder = new QueryBuilder(
                    new Q(query),
                    new FL(new Fields("id")));

                var rendered = queryBuilder.Render(new Builder(SolrEntity<TDelete>.Translations, SolrEntity<TDelete>.DefaultQueries));
                var responseContent = await _solrConnection.GetAsync(rendered, _coreName);
                var response = _serializer.Deserialize<SolrResponse<TDelete>>(responseContent);

                _entitySetter.SetRange<TKey>(response.Response.Docs.Select(entity =>
                    new EntityTransaction
                    {
                        Entity = entity,
                        State = TransactionState.Delete
                    }));

                return await _solrConnection.PostAsync(_serializer.Serialize(response.Response.Docs), _coreName);
            }

            var serialize = GetDeleteQuery(query.Render(new Builder(SolrEntity<T>.Translations, SolrEntity<T>.DefaultQueries)));
            return await _solrConnection.PostAsync(serialize, _coreName);
        }

        public static void SetCoreName(string coreName)
        {
            _coreName = coreName;
        }

        private string GetDeleteQuery(string query)
        {
            return _serializer.Serialize(new Delete
            {
                Value = new DeleteQuery
                {
                    Query = query
                }
            });
        }

        private async Task ConfirmIdNotInUse(T item)
        {
            var idIsInUse = true;
            while (idIsInUse)
            {
                item.NewId();
                var query = new QueryBuilder(
                    new Q(new ByField("id", item.Id)),
                    new Rows(0));

                var result = await GetAsync(query);
                idIsInUse = result.Response.NumFound > 0;
            }
        }

        private async Task ConfirmIdsNotInUse(IEnumerable<T> items)
        {
            var queries = items.Select(x =>
            {
                x.NewId();
                return (IQuery)new ByField("id", x.Id);
            }).ToArray();

            var idsAreInUse = true;
            while (idsAreInUse)
            {
                var query = new QueryBuilder(
                    new Q(new SameOperatorQuery(queries)),
                    new QOperator(),
                    new FL(new Fields("id")));

                var result = await GetAsync(query);
                if (result.Response.Docs.Count > 0)
                {
                    ISet<TKey> idSet = new HashSet<TKey>(result.Response.Docs.Select(x => x.Id));
                    queries = items.Where(x => idSet.Contains(x.Id)).Select(x =>
                    {
                        x.NewId();
                        return (IQuery)new ByField("id", x.Id);
                    }).ToArray();
                    idsAreInUse = true;
                }
                else
                {
                    idsAreInUse = false;
                }
            }
        }
    }
}