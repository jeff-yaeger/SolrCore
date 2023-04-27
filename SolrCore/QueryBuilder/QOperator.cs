namespace SolrCore.QueryBuilder
{
    public class QOperator : Query
    {
        private readonly string _operator;

        public QOperator(QOperatorEnum operatorEnum = QOperatorEnum.Or)
        {
            _operator = operatorEnum == QOperatorEnum.Or ? "OR" : "AND";
        }

        public override string Render(Builder dto)
        {
            return $"q.op={_operator}";
        }
    }
}