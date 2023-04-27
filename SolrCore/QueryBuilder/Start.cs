namespace SolrCore.QueryBuilder
{
    public class Start : Query
    {
        private readonly int _start;

        public Start(int start = 0)
        {
            _start = start;
        }

        public override string Render(Builder dto)
        {
            return $"start={_start}";
        }
    }
}