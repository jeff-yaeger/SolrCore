namespace SolrCore.QueryBuilder
{
    using System.Collections.Generic;
    using System.Text;
    using Models;

    public class Builder
    {
        public readonly IList<DefaultQuery> DefaultQueries;
        public readonly StringBuilder Sb = new StringBuilder();
        public readonly IDictionary<string, string> Translations;

        public Builder(IDictionary<string, string> translations = null, IList<DefaultQuery> defaultQueries = null)
        {
            Translations = translations;
            DefaultQueries = defaultQueries;
        }
    }
}