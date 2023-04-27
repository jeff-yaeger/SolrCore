namespace SolrCore.QueryBuilder
{
    public class Operator : Query
    {
        private readonly IBuilder[] _elements;
        private readonly string _operator;
        private bool _seclude = true;

        protected Operator(string @operator, params IBuilder[] elements)
        {
            _operator = @operator;
            _elements = elements;
        }

        public override void Build(Builder dto)
        {
            if (_seclude)
            {
                dto.Sb.Append('(');
            }

            BuildQuery(dto);

            if (_seclude)
            {
                dto.Sb.Append(')');
            }
        }

        public override string Render(Builder dto)
        {
            _seclude = false;
            return Render(Build, dto);
        }

        private void BuildQuery(Builder dto)
        {
            for (var i = 0; i < _elements.Length; i++)
            {
                _elements[i].Build(dto);
                if (i != _elements.Length - 1)
                {
                    dto.Sb.Append(_operator);
                }
            }
        }
    }
}