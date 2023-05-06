namespace SolrCoreTests
{
    using SolrCore;
    using SolrCore.Models;
    using SolrCore.Query;
    using SolrCore.QueryBuilder;

    public class QueryBuilderTests
    {
        private Builder _builder;
        private List<DefaultQuery> _defaultQueries;
        private Dictionary<string, string> _translations;

        [SetUp]
        public void Setup()
        {
            _translations = new Dictionary<string, string>
            {
                { "Name", "name_s" },
                { "Type", "type_i" },
                { "Price", "price_d" },
                { "Date", "date_dt" },
                { "Active", "active_b" }
            };
            _defaultQueries = new List<DefaultQuery> { new(QOperatorEnum.And, new ByField("Active", "true")) };
        }

        [Test]
        public void Render_WhenCalled_ReturnsQueryString()
        {
            _builder = new Builder(_translations, _defaultQueries);

            var date = new DateTime(2023, 4, 22, 12, 0, 0);
            var queryBuilder = new QueryBuilder(
                new Q(new ByRange("Date", date.AddDays(-2).ToSolrValue(), date.ToSolrValue()).And(
                    new ByField("Name", "Popsicles").And(
                        new ByField("Price", 5.0)).Boost(1.3).Or(
                        new ByField("Name", "Bicycles").And(
                            new ByField("Type", 3).Not())))
                ),
                new FQ(new ByField("Price", "99.0")),
                new FQ(new ByField("Type", "1")),
                new FL(
                    new Fields("Name", "Price", "Random"),
                    new Child(
                        new ChildFilter(
                            new ByRange<ItemChild>(nameof(ItemChild.CreatedDate), "NOW-2DAYS", "NOW").Or(
                                new ByField<ItemChild>(nameof(ItemChild.Name), "Bob").And(
                                    new ByField<ItemChild>(nameof(ItemChild.Type), 15)))
                        ),
                        new Limit(10),
                        new Sort<ItemChild>(nameof(ItemChild.Price), SortEnum.Desc),
                        new FL(new Fields<ItemChild>(nameof(ItemChild.Price), nameof(ItemChild.Type)))
                    )
                ),
                new Sort("Name", SortEnum.Desc),
                new QOperator(QOperatorEnum.And),
                new Start(1),
                new Rows(10),
                new Indent(true),
                new SameOperatorQuery(
                    new ByField("id", "1"),
                    new ByField("id", "2")
                ),
                new SameOperatorQuery(
                    new ByField("id", "3"),
                    new ByField("id", "Cheese")
                )
            );

            var result = queryBuilder.Render(_builder);
            const string compare =
                "?fl=name_s,price_d,Random,[child childFilter=(child_createdDate_dt:[NOW-2DAYS TO NOW] OR (child_name_s:Bob AND child_type_i:15)) limit=10 sort=child_price_d desc fl=child_price_d,child_type_i]&fq=price_d:99.0&fq=type_i:1&q.op=AND&q=((date_dt:[2023-04-20T12:00:00Z TO 2023-04-22T12:00:00Z] AND ((name_s:Popsicles AND price_d:5)^1.3 OR (name_s:Bicycles AND  NOT type_i:3))) AND active_b:true)&rows=10&sort=name_s desc&start=1&indent=true&id:\"1\"id:\"2\"&id:\"3\"id:\"Cheese\"";

            Assert.That(Builder.InstanceCount, Is.EqualTo(1));
            Builder.InstanceCount = 0;
            Assert.That(result, Is.EqualTo(compare));
        }

        [Test]
        public void Render_WhenCalled_ReturnsParentQueryString()
        {
            _builder = new Builder(_translations, _defaultQueries, true);

            var queryBuilder = new QueryBuilder(
                new Q(
                    new ParentWhich(
                        new ByField("Name", "Popsicles"),
                        new MinusNestPath(new Path("path", "to", "child")),
                        new PlusNestPath(new Path("different", "path", "to", "child"), new ByField("Price", "99.0"))
                    ))
            );

            var result = queryBuilder.Render(_builder);
            const string compare =
                "?q={!parent which=\"name_s:Popsicles -_nest_path_:\\\\/path\\\\/to\\\\/child\\\\/*\"}(%2B_nest_path_:\\/different\\/path\\/to\\/child %2Bprice_d:99.0)&rows=100000&indent=false";

            Assert.That(Builder.InstanceCount, Is.EqualTo(1));
            Builder.InstanceCount = 0;
            Assert.That(result, Is.EqualTo(compare));
        }

        [Test]
        public void Render_WhenCalled_ReturnsBasicParentQueryString()
        {
            _builder = new Builder(_translations, _defaultQueries, true);

            var queryBuilder = new QueryBuilder(
                new Q(
                    new ParentWhich(
                        minusNestPath: new MinusNestPath(new Path()),
                        plusNestPath: new PlusNestPath(new Path("path"), new ByField("Price", "98.0"))
                    ))
            );

            var result = queryBuilder.Render(_builder);
            const string compare =
                "?q={!parent which=\"*:* -_nest_path_:*\"}(%2B_nest_path_:\\/path %2Bprice_d:98.0)&rows=100000&indent=false";

            Assert.That(Builder.InstanceCount, Is.EqualTo(1));
            Builder.InstanceCount = 0;
            Assert.That(result, Is.EqualTo(compare));
        }

        [Test]
        public void Render_WhenCalled_ReturnsChildQueryString()
        {
            _builder = new Builder(_translations, _defaultQueries, true);

            var queryBuilder = new QueryBuilder(
                new Q(
                    new ChildOf(
                        new ByField("Name", "Popsicles"),
                        new MinusNestPath(new Path("path", "to", "child")),
                        new PlusNestPath(new Path("different", "path", "to", "child"), new ByField("Price", "1.01"))
                    ))
            );

            var result = queryBuilder.Render(_builder);
            const string compare =
                "?q={!child of=\"name_s:Popsicles -_nest_path_:\\\\/path\\\\/to\\\\/child\\\\/*\"}(%2B_nest_path_:\\/different\\/path\\/to\\/child %2Bprice_d:1.01)&rows=100000&indent=false";

            Assert.That(Builder.InstanceCount, Is.EqualTo(1));
            Builder.InstanceCount = 0;
            Assert.That(result, Is.EqualTo(compare));
        }
    }
}