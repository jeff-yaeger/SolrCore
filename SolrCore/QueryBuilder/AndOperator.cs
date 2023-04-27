namespace SolrCore.QueryBuilder
{
    public class AndOperator : Operator
    {
        public AndOperator(params IBuilder[] elements) : base(" AND ", elements)
        {
        }
    }
}