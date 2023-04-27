namespace SolrCoreWeb.Models
{
    using EntityModels;
    using Newtonsoft.Json;
    using SolrCore.Models;

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ItemUpdate : SolrEntity<ItemUpdate>, IEntity<string>, IOnDelete<string>, IOnUpdate<string>
    {
        [SolrFieldName("lastUpdatedUserId_s")]
        public IUpdate? LastUpdatedUserId { get; set; }

        [SolrFieldName("lastUpdatedDate_dt")]
        public IUpdate? LastUpdatedDate { get; set; }

        [SolrFieldName("active_b")]
        public IUpdate? Active { get; set; }

        [SolrFieldName("type_i")]
        public IUpdate? Type { get; set; }

        [SolrFieldName("price_d")]
        public IUpdate? Price { get; set; }

        [SolrFieldName("name_s")]
        public IUpdate? Name { get; set; }

        [SolrFieldName("id")]
        public string Id { get; set; }

        public void OnDelete(string id, params object[] objects)
        {
            OnUpdate(id, objects);
            Active = new Set(false);
        }

        public void OnUpdate(string id, params object[] objects)
        {
            if (!string.IsNullOrEmpty(id))
            {
                LastUpdatedUserId = new Set(id);
            }

            LastUpdatedDate = new Set(DateTime.UtcNow);
        }
    }
}