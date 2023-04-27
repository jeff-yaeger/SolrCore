namespace SolrCore.Models
{
    public class ResponseHeader
    {
        [SolrFieldName("status")]
        public int Status { get; set; }
    }
}