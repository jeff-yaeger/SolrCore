namespace SolrCoreWeb.Models
{
    using EntityModels;
    using SolrCore.Models;

    public class OtherItem : SolrEntity<OtherItem>, IEntity<string>, IOnAdd<string>, ISolrId
    {
        static OtherItem()
        {
            SetStandardDefaults();
        }

        [SolrFieldName("description_s")]
        public string? Description { get; set; }

        [SolrFieldName("type_i")]
        public int? Type { get; set; }

        [SolrFieldName("price_d")]
        public double? Price { get; set; }

        [SolrFieldName("createdDate_dt")]
        public DateTime CreatedDate { get; set; }

        [SolrFieldName("createdUserId_s")]
        public string? CreatedUserId { get; set; }

        [SolrFieldName("id")]
        public string? Id { get; set; }

        [SolrFieldName("lastUpdatedDate_dt")]
        public DateTime LastUpdatedDate { get; set; }

        [SolrFieldName("lastUpdatedUserId_s")]
        public string? LastUpdatedUserId { get; set; }

        [SolrFieldName("name_s")]
        public string? Name { get; set; }

        [SolrFieldName("active_b")]
        public bool Active { get; set; }

        public void NewId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void OnAdd(string id, params object[] objects)
        {
            NewId();
            Active = true;
            CreatedDate = DateTime.UtcNow;
            LastUpdatedDate = DateTime.UtcNow;
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            CreatedUserId = id;
            LastUpdatedUserId = id;
        }
    }
}