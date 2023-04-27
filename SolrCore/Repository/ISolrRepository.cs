namespace SolrCore.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EntityModels;
    using Models;
    using Query;
    using QueryBuilder;

    public interface ISolrRepository<TKey, T> where T : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        Task<bool> AddAsync(T item);
        Task<bool> AddRangeAsync(IEnumerable<T> items);
        Task<SolrResponse<T>> GetAsync(IQuery query);
        Task<bool> UpdateAsync<TUpdate>(TUpdate item) where TUpdate : SolrEntity<TUpdate>, IEntity<TKey>;
        Task<bool> UpdateRangeAsync<TUpdate>(IEnumerable<TUpdate> items) where TUpdate : SolrEntity<TUpdate>, IEntity<TKey>;
        Task<bool> DeleteAsync<TDelete>(TKey id) where TDelete : class, IEntity<TKey>, new();
        Task<bool> DeleteAsync<TDelete>(Query query) where TDelete : SolrEntity<TDelete>, IEntity<TKey>;
        Task<bool> DeleteAllAsync<TDelete>() where TDelete : SolrEntity<TDelete>, IEntity<TKey>;
    }
}