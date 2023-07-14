namespace SolrCore.Models
{
    using System;

    public class IdResponse<TKey> where TKey: IEquatable<TKey>
    {
        [SolrFieldName("id")]
        public TKey Id { get; set; }
    }
}