namespace SolrCore.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EntityModels;
    using Models;
    using Query;
    using QueryBuilder;

    public interface ISolrRepository<TKey, T> where T : class where TKey : IEquatable<TKey>
    {
        Task<bool> AddAsync(T item, bool commit = true);
        Task<bool> AddRangeAsync(IEnumerable<T> items, bool commit = true);
        Task<SolrResponse<T>> GetAsync(IQuery query, bool ignoreDefaultQueries = false);
        Task<bool> UpdateAsync<TUpdate>(TUpdate item, bool commit = true) where TUpdate : class;
        Task<bool> UpdateRangeAsync<TUpdate>(IEnumerable<TUpdate> items, bool commit = true) where TUpdate : class;
        Task<bool> DeleteAsync<TDelete>(TKey id, bool commit = true) where TDelete : class, IEntity<TKey>, new();
        Task<bool> DeleteAsync<TDelete>(Query query, bool commit = true) where TDelete : SolrEntity<TDelete>, IEntity<TKey>;
        Task<bool> DeleteAllAsync<TDelete>(bool commit = true) where TDelete : SolrEntity<TDelete>, IEntity<TKey>;
        Task<bool> UpdateAsync(T item, bool commit = true);
        Task<bool> UpdateRangeAsync(IEnumerable<T> items, bool commit = true);
        Task<SolrResponse<TResult>> GetAsync<TResult>(IQuery query, bool ignoreDefaultQueries = false) where TResult : class;
    }
}