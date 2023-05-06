namespace SolrCore.QueryBuilder
{
    public class MinusNestPath : Query
    {
        private readonly Path _path;

        public MinusNestPath(Path path)
        {
            path.ChangeStyle(NestPathStyle.Minus);
            _path = path;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            dto.Sb.Append("-_nest_path_:");
            _path.Build(dto);
        }
    }
}