namespace SolrCore.QueryBuilder
{
    using System.Collections.Generic;
    using System.Text;
    using Models;

    public class Builder
    {
#if DEBUG
        public static int InstanceCount;
#endif
        public readonly IList<DefaultQuery> DefaultQueries;
        public readonly StringBuilder Sb = new StringBuilder();
        public readonly IDictionary<string, string> Translations;
        public readonly bool IgnoreDefaultQueries;

        public Builder(IDictionary<string, string> translations = null, IList<DefaultQuery> defaultQueries = null, bool ignoreDefaultQueries = false)
        {
            IgnoreDefaultQueries = ignoreDefaultQueries;
            Translations = translations;
            DefaultQueries = defaultQueries;

#if DEBUG
            InstanceCount += 1;
#endif
        }
    }
}