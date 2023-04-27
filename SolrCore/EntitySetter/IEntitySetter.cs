namespace SolrCore.EntitySetter
{
    using System;
    using System.Collections.Generic;
    using EntityDefaults;

    public interface IEntitySetter
    {
        void Set<TKey>(EntityTransaction entityTransaction) where TKey : IEquatable<TKey>;
        void SetRange<TKey>(IEnumerable<EntityTransaction> entities) where TKey : IEquatable<TKey>;
    }
}