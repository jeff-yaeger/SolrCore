namespace SolrCore.Query
{
    using QueryBuilder;

    public class AllQuery : Query
    {
        public override string Render(Builder dto)
        {
            return "*:*";
        }
    }
}