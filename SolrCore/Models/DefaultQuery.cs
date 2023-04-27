namespace SolrCore.Models
{
    using QueryBuilder;

    public class DefaultQuery
    {
        public DefaultQuery(QOperatorEnum operatorEnum, Query query)
        {
            OperatorEnum = operatorEnum;
            Query = query;
        }

        public QOperatorEnum OperatorEnum { get; set; }
        public Query Query { get; set; }
    }
}