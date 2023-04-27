namespace SolrCore.Models
{
    public class SolrResponse<T>
    {
        [SolrFieldName("responseHeader")]
        public ResponseHeader ResponseHeader { get; set; }

        [SolrFieldName("response")]
        public DocResponse<T> Response { get; set; }
    }
}