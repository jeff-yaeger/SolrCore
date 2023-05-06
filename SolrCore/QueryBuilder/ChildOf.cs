namespace SolrCore.QueryBuilder
{
    using SolrCore.Query;

    public class ChildOf : Query
    {
        private readonly MinusNestPath _minusNestPath;
        private readonly PlusNestPath _plusNestPath;
        private readonly Query _query;

        public ChildOf(Query query = null, MinusNestPath minusNestPath = null, PlusNestPath plusNestPath = null)
        {
            if (query == null)
            {
                query = new AllQuery();
            }

            _query = query;
            _minusNestPath = minusNestPath;
            _plusNestPath = plusNestPath;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            dto.Sb.Append("{!child of=\"");
            _query.Build(dto);
            dto.Sb.Append(" ");
            _minusNestPath?.Build(dto);
            dto.Sb.Append("\"}");
            _plusNestPath?.Build(dto);
        }
    }
}