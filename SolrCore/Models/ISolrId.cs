namespace SolrCore.Models
{
    using System;
    using EntityModels;

    public interface ISolrId<TKey>:IEntity<TKey> where TKey:IEquatable<TKey>
    {
        void NewId();
    }
}