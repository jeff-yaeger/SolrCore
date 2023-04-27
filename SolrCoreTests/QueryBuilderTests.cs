namespace SolrCoreTests
{
    using SolrCore;
    using SolrCore.Models;
    using SolrCore.Query;
    using SolrCore.QueryBuilder;

    public class QueryBuilderTests
    {
        private Builder _builder;

        [SetUp]
        public void Setup()
        {
            var translations = new Dictionary<string, string>
            {
                { "Name", "name_s" },
                { "Type", "type_i" },
                { "Price", "price_d" },
                { "Date", "date_dt" },
                { "Active", "active_b" }
            };
            var defaultQueries = new List<DefaultQuery> { new(QOperatorEnum.And, new ByField("Active", "true")) };
            _builder = new Builder(translations, defaultQueries);
        }

        [Test]
        public void Render_WhenCalled_ReturnsQueryString()
        {
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
                            new ByField("Name", "Bob")
                        ),
                        new Limit(10),
                        new FL(new Fields("Name"))
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
            // const string compare =
            // "?fl=name_s,price_d,Random,[child childFilter=name_s:\"Bob\" limit=10 fl=name_s]&fq=price_d:\"99.0\"&fq=type_i:\"1\"&q.op=AND&q=((date_dt:[2023-04-20T12:00:00Z TO 2023-04-22T12:00:00Z] AND ((name_s:\"Popsicles\" AND price_d:\"5\")^1.3 OR (name_s:\"Bicycles\" AND  NOT type_i:\"3\")) AND active_b:\"true\")&rows=10&sort=name_s desc&start=1&indent=true&id:\"1\"id:\"2\"&id:\"3\"id:\"Cheese\"";
            const string compare =
                "?fl=name_s,price_d,Random,[child childFilter=name_s:\"Bob\" limit=10 fl=name_s]&fq=price_d:\"99.0\"&fq=type_i:\"1\"&q.op=AND&q=((date_dt:[2023-04-20T12:00:00Z TO 2023-04-22T12:00:00Z] AND ((name_s:\"Popsicles\" AND price_d:\"5\")^1.3 OR (name_s:\"Bicycles\" AND  NOT type_i:\"3\"))) AND active_b:\"true\")&rows=10&sort=name_s desc&start=1&indent=true&id:\"1\"id:\"2\"&id:\"3\"id:\"Cheese\"";
            Assert.That(result, Is.EqualTo(compare));
        }
    }
}