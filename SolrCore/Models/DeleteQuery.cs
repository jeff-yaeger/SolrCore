namespace SolrCore.Models
{
    public class DeleteQuery
    {
        [SolrFieldName("query")]
        public object Query { get; set; }
    }
}