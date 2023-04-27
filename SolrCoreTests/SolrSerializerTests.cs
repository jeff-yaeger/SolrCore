namespace SolrCoreTests
{
    using System.Text.Json.Serialization;
    using SolrCore.Models;
    using SolrCore.Serializer;

    public class SolrSerializerTests
    {
        private const string Serialized = "{\"type_i\":3,\"price_d\":2.1,\"id\":\"800eb4bc-b196-45f1-af36-78dcbb4cc388\",\"name_s\":\"name\"}";

        private const string AtomicUpdateSerialized =
            "{\"name_s\":{\"set\":\"nameUpdate\"},\"description_s\":{\"remove\":2},\"active_b\":{\"inc\":-1},\"type_i\":{\"add-distinct\":2},\"price_d\":{\"add\":2.2},\"id\":\"800eb4bc-b196-45f1-af36-78dcbb4cc388\"}";

        private ItemUpdate _atomicUpdateNewtonsoftItem;
        private Item _item;
        private ISolrSerializer _serializer;

        [SetUp]
        public void Setup()
        {
            _item = new Item
            {
                Id = "800eb4bc-b196-45f1-af36-78dcbb4cc388",
                Name = "name",
                Price = 2.1,
                Type = 3
            };

            _atomicUpdateNewtonsoftItem = new ItemUpdate
            {
                Id = "800eb4bc-b196-45f1-af36-78dcbb4cc388",
                Active = new Increment(-1),
                Name = new Set("nameUpdate"),
                Price = new Add(2.2),
                Type = new AddDistinct(2),
                Description = new Remove(2)
            };

            _serializer = new SolrSerializer();
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
            var result = _serializer.Deserialize<Item>(Serialized);

            Assert.Multiple(() =>
            {
                Assert.That(result.Id, Is.EqualTo("800eb4bc-b196-45f1-af36-78dcbb4cc388"));
                Assert.That(result.Name, Is.EqualTo("name"));
                Assert.That(result.Price, Is.EqualTo(2.1));
                Assert.That(result.Type, Is.EqualTo(3));
            });
        }

        [Test]
        [Ignore("Haven't figured out work around to get this to serialize IUpdate correctly.")]
        public void Serialize_WhenCalled_ReturnsAtomicUpdateObjectSerialized()
        {
            var result = _serializer.Serialize(_atomicUpdateNewtonsoftItem);
            Assert.That(result, Is.EqualTo(AtomicUpdateSerialized));
        }
    }

    public class Item
    {
        [SolrFieldName("type_i")]
        public int? Type { get; set; }

        [SolrFieldName("price_d")]
        public double? Price { get; set; }

        [SolrFieldName("id")]
        public string? Id { get; set; }

        [SolrFieldName("name_s")]
        public string Name { get; set; }
    }

    public class ItemUpdate
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
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IUpdate ShouldBeIgnored { get; set; }

        [SolrFieldName("id")]
        public string Id { get; set; }
    }
}