namespace SolrCore.QueryBuilder
{
    using SolrCore.Query;

    public class SameOperatorQuery : Query
    {
        private readonly IQuery[] _queries;

        public SameOperatorQuery(params IQuery[] queries)
        {
            _queries = queries;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            foreach (var query in _queries)
            {
                query.Build(dto);
            }
        }
    }
}