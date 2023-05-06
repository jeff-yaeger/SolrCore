namespace SolrCore.QueryBuilder
{
    using SolrCore.Query;

    public class PlusNestPath : Query
    {
        private readonly Path _path;
        private readonly IQuery[] _queries;

        public PlusNestPath(Path path, params IQuery[] queries)
        {
            _path = path;
            _queries = queries;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            // + not encoding properly using %2b instead
            dto.Sb.Append("(%2B_nest_path_:");
            _path.Build(dto);
            if (_queries != null && _queries.Length > 0)
            {
                foreach (var query in _queries)
                {
                    dto.Sb.Append(" %2B");
                    query.Build(dto);
                }
            }

            dto.Sb.Append(")");
        }
    }
}