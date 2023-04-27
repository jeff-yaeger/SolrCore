namespace SolrCore.Models
{
    public class Delete
    {
        [SolrFieldName("delete")]
        public object Value { get; set; }
    }
}