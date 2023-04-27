namespace SolrCore.QueryBuilder
{
    using SolrCore.Query;

    public class FL : Query
    {
        private readonly Fields _fields;
        private readonly IQuery[] _queries;

        public FL(Fields fields = null, params IQuery[] queries)
        {
            _fields = fields;
            _queries = queries;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            dto.Sb.Append("fl=");
            if (_fields != null)
            {
                _fields.Build(dto);
            }
            else
            {
                dto.Sb.Append('*');
            }

            if (_queries != null && _queries.Length > 0)
            {
                dto.Sb.Append(',');
                for (var i = 0; i < _queries.Length; i++)
                {
                    _queries[i].Build(dto);
                    if (i != _queries.Length - 1)
                    {
                        dto.Sb.Append(',');
                    }
                }
            }
        }
    }
}