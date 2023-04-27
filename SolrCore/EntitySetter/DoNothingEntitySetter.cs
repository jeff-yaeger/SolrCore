namespace SolrCore.EntitySetter
{
    using System;
    using System.Collections.Generic;
    using EntityDefaults;

    public class DoNothingEntitySetter : IEntitySetter
    {
        public void Set<TKey>(EntityTransaction entityTransaction) where TKey : IEquatable<TKey>
        {
        }

        public void SetRange<TKey>(IEnumerable<EntityTransaction> entities) where TKey : IEquatable<TKey>
        {
        }
    }
}