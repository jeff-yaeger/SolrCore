namespace SolrCore.QueryBuilder
{
    using SolrCore.Query;

    public class ChildFilter : Query
    {
        private readonly IQuery _query;

        public ChildFilter(IQuery query)
        {
            _query = query;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            dto.Sb.Append("childFilter=");
            _query.Build(dto);
        }
    }
}