namespace SolrCore.QueryBuilder
{
    using SolrCore.Query;

    public class NotOperator : Query
    {
        private readonly IQuery _query;

        public NotOperator(IQuery query)
        {
            _query = query;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            dto.Sb.Append(" NOT ");
            _query.Build(dto);
        }
    }
}