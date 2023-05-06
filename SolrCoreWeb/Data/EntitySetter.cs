namespace SolrCoreWeb.Data
{
    using System.Security.Claims;
    using EntityDefaults;
    using EntityModels;

    public class EntitySetter : IEntitySetter
    {
        private readonly IHttpContextAccessor _accessor;

        public EntitySetter(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public void Set<TKey>(EntityTransaction entityTransaction) where TKey : IEquatable<TKey>
        {
            var id = GetCurrentUsersId<TKey>();
            if (entityTransaction.Entity is IOnAdd<TKey> or IOnUpdate<TKey> or IOnDelete<TKey>)
            {
                entityTransaction.SetDefaults(id);
            }
            else
            {
                var defaultDataSetters = GetDefaultDataSetters(id);
                SetDefaults(entityTransaction, defaultDataSetters);
            }
        }

        public void SetRange<TKey>(IEnumerable<EntityTransaction> entities) where TKey : IEquatable<TKey>
        {
            var id = GetCurrentUsersId<TKey>();
            var defaultDataSetters = GetDefaultDataSetters(id);
            foreach (var entityTransaction in entities)
            {
                if (entityTransaction.Entity is IOnAdd<TKey> or IOnUpdate<TKey> or IOnDelete<TKey>)
                {
                    entityTransaction.SetDefaults(id);
                }
                else
                {
                    SetDefaults(entityTransaction, defaultDataSetters);
                }
            }
        }

        private static IDefaultEntitySetter[] GetDefaultDataSetters<TKey>(TKey? id) where TKey : IEquatable<TKey>
        {
            var defaultDataSetters = new IDefaultEntitySetter[]
            {
                new SoftDeleteSetter(),
                new CreateDateSetter(),
                new CreatedUserIdSetter<TKey>(id),
                new LastUpdateDateSetter(),
                new LastUpdatedUserIdSetter<TKey>(id),
                new NormalizeNameSetter()
            };
            return defaultDataSetters;
        }

        private static void SetDefaults(EntityTransaction entityTransaction, IEnumerable<IDefaultEntitySetter> defaultDataSetters)
        {
            foreach (var setter in defaultDataSetters)
            {
                setter.Set(entityTransaction);
            }
        }

        private TKey? GetCurrentUsersId<TKey>() where TKey : IEquatable<TKey>
        {
            var userId = _accessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            TKey id = default;
            if (!string.IsNullOrEmpty(userId))
            {
                id = (TKey)Convert.ChangeType(userId, typeof(TKey));
            }

            return id;
        }
    }
}