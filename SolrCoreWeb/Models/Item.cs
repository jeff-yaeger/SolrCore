namespace SolrCoreWeb.Models
{
    using EntityModels;
    using Newtonsoft.Json;
    using SolrCore.Models;

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Item : SolrEntity<Item>, IEntity<string>, IName, ICreated<string>, ILastUpdated<string>, ISolrId, ISoftDelete
    {
        static Item()
        {
            SetStandardDefaults();
        }

        [SolrFieldName("description_s")]
        public string Description { get; set; }

        [SolrFieldName("type_i")]
        public int? Type { get; set; }

        [SolrFieldName("price_d")]
        public double? Price { get; set; }

        [SolrFieldName("child")]
        public SolrChild? Child { get; set; }

        [SolrFieldName("strings_ss")]
        public IEnumerable<string>? Strings { get; set; }

        [SolrFieldName("ints_is")]
        public IEnumerable<int>? Ints { get; set; }

        [SolrFieldName("dateTimes_dts")]
        public IEnumerable<DateTime?>? DateTimes { get; set; }

        [SolrFieldName("children")]
        public IEnumerable<SolrChild2>? Children { get; set; }

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
        public string Name { get; set; }

        [SolrFieldName("active_b")]
        public bool Active { get; set; }

        public void NewId()
        {
            Id = Guid.NewGuid().ToString();
            Child?.NewId();
            if (Children != null && Children.Any())
            {
                foreach (var child in Children)
                {
                    child.NewId();
                }
            }
        }
    }

    public class SolrChild : SolrEntity<SolrChild>, IEntity<string>, ISolrId
    {
        [SolrFieldName("type_i")]
        public int? Type { get; set; }

        [SolrFieldName("price_d")]
        public double? Price { get; set; }

        [SolrFieldName("name_s")]
        public string? Name { get; set; }

        [SolrFieldName("id")]
        public string? Id { get; set; }

        public void NewId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class SolrChild2 : SolrEntity<SolrChild2>, IEntity<string>, ISolrId
    {
        [SolrFieldName("type_i")]
        public int? Type { get; set; }

        [SolrFieldName("price_d")]
        public double? Price { get; set; }

        [SolrFieldName("name_s")]
        public string? Name { get; set; }

        [SolrFieldName("child")]
        public SolrChild3? Child { get; set; }

        [SolrFieldName("id")]
        public string? Id { get; set; }

        public void NewId()
        {
            Id = Guid.NewGuid().ToString();
            Child?.NewId();
        }
    }

    public class SolrChild3 : SolrEntity<SolrChild3>, IEntity<string>, ISolrId
    {
        [SolrFieldName("child")]
        public SolrChild? Child { get; set; }

        [SolrFieldName("children")]
        public ICollection<SolrChild>? Children { get; set; }

        [SolrFieldName("type_i")]
        public int? Type { get; set; }

        [SolrFieldName("price_d")]
        public double? Price { get; set; }

        [SolrFieldName("name_s")]
        public string? Name { get; set; }

        [SolrFieldName("id")]
        public string? Id { get; set; }

        public void NewId()
        {
            Id = Guid.NewGuid().ToString();
            Child?.NewId();
            if (Children != null && Children.Any())
            {
                foreach (var child in Children)
                {
                    child.NewId();
                }
            }
        }
    }
}