namespace SolrCore.Models
{
    using System.Collections.Generic;

    public class DocResponse<T>
    {
        [SolrFieldName("numFound")]
        public int NumFound { get; set; }

        [SolrFieldName("start")]
        public int Start { get; set; }

        [SolrFieldName("numFoundExact")]
        public bool NumFoundExact { get; set; }

        [SolrFieldName("docs")]
        public List<T> Docs { get; set; }
    }
}