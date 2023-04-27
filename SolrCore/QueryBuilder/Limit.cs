namespace SolrCore.QueryBuilder
{
    public class Limit : Query
    {
        private readonly int _limit;

        public Limit(int limit)
        {
            _limit = limit;
        }

        public override string Render(Builder dto)
        {
            return $"limit={_limit}";
        }
    }
}