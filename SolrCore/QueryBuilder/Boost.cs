namespace SolrCore.QueryBuilder
{
    using SolrCore.Query;

    public class Boost : Query
    {
        private readonly double _boost;
        private readonly IQuery _query;

        public Boost(IQuery query, double boost)
        {
            _query = query;
            _boost = boost;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            _query.Build(dto);
            dto.Sb.Append($"^{_boost:F1}");
        }
    }
}