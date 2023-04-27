namespace SolrCore.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using EntityModels;
    using Query;
    using QueryBuilder;

    public abstract class SolrEntity<T> where T : class
    {
        public static readonly IDictionary<string, string> Translations;
        public static readonly IList<DefaultQuery> DefaultQueries = new List<DefaultQuery>();

        static SolrEntity()
        {
            Translations = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute(typeof(SolrFieldNameAttribute)) != null)
                .ToDictionary(x => x.Name, x => ((SolrFieldNameAttribute)x.GetCustomAttribute(typeof(SolrFieldNameAttribute))).Name);
        }

        protected static void SetStandardDefaults()
        {
            if (typeof(ISoftDelete).IsAssignableFrom(typeof(T)))
            {
                DefaultQueries.Add(new DefaultQuery(QOperatorEnum.And, new ByField(nameof(ISoftDelete.Active), true.ToSolrValue())));
            }
        }
    }
}