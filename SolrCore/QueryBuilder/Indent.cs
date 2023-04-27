namespace SolrCore.QueryBuilder
{
    public class Indent : Query
    {
        private readonly string _indent;

        public Indent(bool indent = false)
        {
            _indent = indent.ToSolrValue();
        }

        public override string Render(Builder dto)
        {
            return $"indent={_indent}";
        }
    }
}