namespace SolrCore.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Connection;
    using EntityDefaults;
    using EntityModels;
    using Models;
    using Query;
    using QueryBuilder;
    using Serializer;

    public class SolrRepository<TKey, T> : ISolrRepository<TKey, T> where T : class where TKey : IEquatable<TKey>
    {
        private static string _coreName;
        private static bool _useOverwriteProtection;
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

        public async Task<bool> AddAsync(T item, bool commit = true)
        {
            _entitySetter.Set<TKey>(new EntityTransaction { Entity = item, State = TransactionState.Add });

            if (_useOverwriteProtection && item is ISolrId<TKey> solrIdItem)
            {
                await ConfirmIdNotInUse(solrIdItem);
            }

            return await _solrConnection.PostAsync(_serializer.Serialize(new[] { item }), _coreName, commit);
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> items, bool commit = true)
        {
            _entitySetter.SetRange<TKey>(items.Select(entity =>
                new EntityTransaction
                {
                    Entity = entity,
                    State = TransactionState.Add
                }));

            if (_useOverwriteProtection && items is IEnumerable<ISolrId<TKey>> solrIdItems)
            {
                await ConfirmIdsNotInUse(solrIdItems);
            }

            return await _solrConnection.PostAsync(_serializer.Serialize(items), _coreName, commit);
        }

        public async Task<bool> UpdateAsync(T item, bool commit = true)
        {
            return await UpdateAsync<T>(item, commit);
        }
        
        public async Task<bool> UpdateAsync<TUpdate>(TUpdate item, bool commit = true) where TUpdate : class
        {
            _entitySetter.Set<TKey>(new EntityTransaction { Entity = item, State = TransactionState.Update });
            return await _solrConnection.PostAsync(_serializer.Serialize(new[] { item }), _coreName, commit);
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<T> items, bool commit = true)
        {
            return await UpdateRangeAsync<T>(items, commit);
        }
        
        public async Task<bool> UpdateRangeAsync<TUpdate>(IEnumerable<TUpdate> items, bool commit = true) where TUpdate : class        
        {
            _entitySetter.SetRange<TKey>(items.Select(entity =>
                new EntityTransaction
                {
                    Entity = entity,
                    State = TransactionState.Update
                }));

            return await _solrConnection.PostAsync(_serializer.Serialize(items), _coreName, commit);
        }

        public async Task<SolrResponse<T>> GetAsync(IQuery query, bool ignoreDefaultQueries = false)
        {
            return await GetAsync<T>(query, new Builder(SolrEntity<T>.Translations, SolrEntity<T>.DefaultQueries, ignoreDefaultQueries));
        }

        public async Task<SolrResponse<TResult>> GetAsync<TResult>(IQuery query, bool ignoreDefaultQueries = false) where TResult : class
        {
            var builder = typeof(SolrEntity<>).IsAssignableFrom(typeof(TResult)) 
                ? new Builder(SolrEntity<T>.Translations, SolrEntity<T>.DefaultQueries, ignoreDefaultQueries) 
                : new Builder(ignoreDefaultQueries: ignoreDefaultQueries);
            
            return await GetAsync<TResult>(query, builder);
        }

        public async Task<bool> DeleteAllAsync<TDelete>(bool commit = true) where TDelete : SolrEntity<TDelete>, IEntity<TKey>
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TDelete)) || typeof(IOnDelete<TKey>).IsAssignableFrom(typeof(TDelete)))
            {
                var queryBuilder = new QueryBuilder(
                    new Q(new ParentWhich(minusNestPath: new MinusNestPath(new Path()))),
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
                
                return await _solrConnection.PostAsync(_serializer.Serialize(response.Response.Docs), _coreName, commit);
            }

            var serialize = GetDeleteQuery("*:*");
            return await _solrConnection.PostAsync(serialize, _coreName, commit);
        }

        public async Task<bool> DeleteAsync<TDelete>(TKey id, bool commit = true) where TDelete : class, IEntity<TKey>, new()
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TDelete)) || typeof(IOnDelete<TKey>).IsAssignableFrom(typeof(TDelete)))
            {
                var delete = new TDelete
                {
                    Id = id
                };

                _entitySetter.Set<TKey>(new EntityTransaction { Entity = delete, State = TransactionState.Delete });
                return await _solrConnection.PostAsync(_serializer.Serialize(new[] { delete }), _coreName, commit);
            }

            var serialize = GetDeleteQuery(new ByField("id", id).Render(new Builder(SolrEntity<T>.Translations, SolrEntity<T>.DefaultQueries)));
            return await _solrConnection.PostAsync(serialize, _coreName, commit);
        }

        public async Task<bool> DeleteAsync<TDelete>(Query query, bool commit = true) where TDelete : SolrEntity<TDelete>, IEntity<TKey>
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

                return await _solrConnection.PostAsync(_serializer.Serialize(response.Response.Docs), _coreName, commit);
            }

            var serialize = GetDeleteQuery(query.Render(new Builder(SolrEntity<T>.Translations, SolrEntity<T>.DefaultQueries)));
            return await _solrConnection.PostAsync(serialize, _coreName, commit);
        }

        public static void SetCoreName(string coreName)
        {
            _coreName = coreName;
        }

        public static void SetOverwriteProtection(bool useOverwriteProtection)
        {
            _useOverwriteProtection = useOverwriteProtection;
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

        private async Task<SolrResponse<TResult>> GetAsync<TResult>(IQuery query, Builder builder) where TResult : class
        {
            var rendered = query.Render(builder);
            var responseContent = await _solrConnection.GetAsync(rendered, _coreName);
            return string.IsNullOrEmpty(responseContent) ? new SolrResponse<TResult>() : _serializer.Deserialize<SolrResponse<TResult>>(responseContent);
        }
        
        private async Task ConfirmIdNotInUse<TAdd>(TAdd item)where TAdd:ISolrId<TKey>
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

        private async Task ConfirmIdsNotInUse<TAdd>(IEnumerable<TAdd> items)where TAdd:ISolrId<TKey>
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

                var result = await GetAsync<IdResponse<TKey>>(query);
                if (result.Response.Docs.Count > 0)
                {
                    ISet<TKey> idSet = new HashSet<TKey>(result.Response.Docs.Select(x => x.Id));
                    queries = items.Where(x => idSet.Contains(x.Id)).Select(x =>
                    {
                        x.NewId();
                        return (IQuery)new ByField("id", x.Id);
                    }).ToArray();
                }
                else
                {
                    idsAreInUse = false;
                }
            }
        }
    }
}