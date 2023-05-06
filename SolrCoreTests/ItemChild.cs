namespace SolrCoreTests
{
    using Newtonsoft.Json;
    using SolrCore.Models;

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ItemChild : SolrEntity<ItemChild>
    {
        [SolrFieldName("child_type_i")]
        public int? Type { get; set; }

        [SolrFieldName("child_price_d")]
        public double? Price { get; set; }

        [SolrFieldName("id")]
        public string? Id { get; set; }

        [SolrFieldName("child_name_s")]
        public string Name { get; set; }

        [SolrFieldName("child_createdDate_dt")]
        public DateTime CreatedDate { get; set; }
    }
}