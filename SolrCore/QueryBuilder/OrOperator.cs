namespace SolrCore.QueryBuilder
{
    public class OrOperator : Operator
    {
        public OrOperator(params IBuilder[] elements) : base(" OR ", elements)
        {
        }
    }
}