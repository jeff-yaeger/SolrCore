namespace SolrCore.QueryBuilder
{
    public class Q : Query
    {
        private readonly Query _query;

        public Q(Query query)
        {
            _query = query;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            dto.Sb.Append("q=");
            if (!dto.IgnoreDefaultQueries && dto.DefaultQueries != null && dto.DefaultQueries.Count > 0)
            {
                var newQuery = _query;
                foreach (var defaultQuery in dto.DefaultQueries)
                {
                    newQuery = defaultQuery.OperatorEnum == QOperatorEnum.And ? newQuery.And(defaultQuery.Query) : newQuery.Or(defaultQuery.Query);
                }

                newQuery.Build(dto);
            }
            else
            {
                _query.Build(dto);
            }
        }
    }
}