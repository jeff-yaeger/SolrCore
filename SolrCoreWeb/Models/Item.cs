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

        [SolrFieldName("Strings_ss")]
        public IEnumerable<string>? Strings { get; set; }

        [SolrFieldName("Ints_is")]
        public IEnumerable<int>? Ints { get; set; }

        [SolrFieldName("DateTimes_dts")]
        public IEnumerable<DateTime?>? DateTimes { get; set; }

        [SolrFieldName("child2")]
        public IEnumerable<SolrChild2>? Child2 { get; set; }

        [SolrFieldName("createdDate_dt")]
        public DateTime CreatedDate { get; set; }

        [SolrFieldName("createdUserId_s")]
        public string CreatedUserId { get; set; }

        [SolrFieldName("id")]
        public string? Id { get; set; }

        [SolrFieldName("lastUpdatedDate_dt")]
        public DateTime LastUpdatedDate { get; set; }

        [SolrFieldName("lastUpdatedUserId_s")]
        public string LastUpdatedUserId { get; set; }

        [SolrFieldName("name_s")]
        public string Name { get; set; }

        [SolrFieldName("active_b")]
        public bool Active { get; set; }

        public void NewId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class SolrChild : IEntity<string>
    {
        [SolrFieldName("type_i")]
        public int? Type { get; set; }

        [SolrFieldName("price_d")]
        public double? Price { get; set; }

        [SolrFieldName("id")]
        public string Id { get; set; }
    }

    public class SolrChild2 : IEntity<string>
    {
        [SolrFieldName("somethingElse_s")]
        public string? SomethingElse { get; set; }

        [SolrFieldName("something_s")]
        public string? Something { get; set; }

        [SolrFieldName("something")]
        public SolrChild3? Something1 { get; set; }

        [SolrFieldName("id")]
        public string Id { get; set; }
    }

    public class SolrChild3 : IEntity<string>
    {
        [SolrFieldName("somethingElse1")]
        public SolrChild4 SomethingElse { get; set; }

        [SolrFieldName("something11")]
        public ICollection<SolrChild5> Something { get; set; }

        [SolrFieldName("id")]
        public string Id { get; set; }
    }

    public class SolrChild4 : IEntity<string>
    {
        [SolrFieldName("somethingElse_s")]
        public string? SomethingElse { get; set; }

        [SolrFieldName("something_s")]
        public string? Something { get; set; }

        [SolrFieldName("id")]
        public string Id { get; set; }
    }

    public class SolrChild5 : IEntity<string>
    {
        [SolrFieldName("somethingElse_s")]
        public string? SomethingElse { get; set; }

        [SolrFieldName("something_s")]
        public string? Something { get; set; }

        [SolrFieldName("id")]
        public string Id { get; set; }
    }
}