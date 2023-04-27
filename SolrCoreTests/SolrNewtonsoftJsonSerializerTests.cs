namespace SolrCoreTests
{
    using Newtonsoft.Json;
    using SolrCore.Models;
    using SolrCore.Serializer;

    public class SolrNewtonsoftJsonSerializerTests
    {
        private const string Serialized =
            "{\"type_i\":3,\"price_d\":2.1,\"id\":\"800eb4bc-b196-45f1-af36-78dcbb4cc388\",\"name_s\":\"name\",\"createdDate_dt\":\"2023-04-22T12:00:00Z\"}";

        private const string AtomicUpdateSerialized =
            "{\"name_s\":{\"set\":\"nameUpdate\"},\"description_s\":{\"remove\":2},\"active_b\":{\"inc\":-1},\"type_i\":{\"add-distinct\":2},\"price_d\":{\"add\":2.2},\"id\":\"800eb4bc-b196-45f1-af36-78dcbb4cc388\"}";

        private NewtonsoftItemUpdate _atomicUpdateNewtonsoftItem;
        private NewtonsoftItem _item;
        private ISolrSerializer _serializer;

        [SetUp]
        public void Setup()
        {
            _item = new NewtonsoftItem
            {
                Id = "800eb4bc-b196-45f1-af36-78dcbb4cc388",
                Name = "name",
                Price = 2.1,
                Type = 3,
                CreatedDate = new DateTime(2023, 4, 22, 12, 0, 0)
            };

            _atomicUpdateNewtonsoftItem = new NewtonsoftItemUpdate
            {
                Id = "800eb4bc-b196-45f1-af36-78dcbb4cc388",
                Active = new Increment(-1),
                Name = new Set("nameUpdate"),
                Price = new Add(2.2),
                Type = new AddDistinct(2),
                Description = new Remove(2)
            };

            _serializer = new SolrNewtonsoftJsonSerializer();
        }

        [Test]
        public void Serialize_WhenCalled_ReturnsObjectSerialized()
        {
            var result = _serializer.Serialize(_item);
            Assert.That(result, Is.EqualTo(Serialized));
        }

        [Test]
        public void Deserialize_WhenCalled_ReturnsObjectDeserialized()
        {
            var result = _serializer.Deserialize<NewtonsoftItem>(Serialized);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo("800eb4bc-b196-45f1-af36-78dcbb4cc388"));
                Assert.That(result.Name, Is.EqualTo("name"));
                Assert.That(result.Price, Is.EqualTo(2.1));
                Assert.That(result.Type, Is.EqualTo(3));
                Assert.That(result.CreatedDate, Is.EqualTo(new DateTime(2023, 4, 22, 12, 0, 0)));
            });
        }

        [Test]
        public void Serialize_WhenCalled_ReturnsAtomicUpdateObjectSerialized()
        {
            var result = _serializer.Serialize(_atomicUpdateNewtonsoftItem);
            Assert.That(result, Is.EqualTo(AtomicUpdateSerialized));
        }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class NewtonsoftItem
    {
        [SolrFieldName("type_i")]
        public int? Type { get; set; }

        [SolrFieldName("price_d")]
        public double? Price { get; set; }

        [SolrFieldName("id")]
        public string? Id { get; set; }

        [SolrFieldName("name_s")]
        public string Name { get; set; }

        [SolrFieldName("createdDate_dt")]
        public DateTime CreatedDate { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class NewtonsoftItemUpdate
    {
        [SolrFieldName("name_s")]
        public IUpdate Name { get; set; }

        [SolrFieldName("description_s")]
        public IUpdate Description { get; set; }

        [SolrFieldName("active_b")]
        public IUpdate Active { get; set; }

        [SolrFieldName("type_i")]
        public IUpdate Type { get; set; }

        [SolrFieldName("price_d")]
        public IUpdate Price { get; set; }

        [SolrFieldName("shouldBeIgnored_txt")]
        public IUpdate ShouldBeIgnored { get; set; }

        [SolrFieldName("id")]
        public string Id { get; set; }
    }
}