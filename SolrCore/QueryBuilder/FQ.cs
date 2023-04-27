namespace SolrCore.QueryBuilder
{
    using SolrCore.Query;

    public class FQ : Query
    {
        private readonly IQuery[] _queries;

        public FQ(params IQuery[] queries)
        {
            _queries = queries;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            dto.Sb.Append("fq=");
            foreach (var query in _queries)
            {
                query.Build(dto);
            }
        }
    }
}