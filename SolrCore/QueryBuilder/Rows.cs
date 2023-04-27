namespace SolrCore.QueryBuilder
{
    public class Rows : Query
    {
        private readonly int _rows;

        public Rows(int rows = 100_000)
        {
            _rows = rows;
        }

        public override string Render(Builder dto)
        {
            return $"rows={_rows}";
        }
    }
}