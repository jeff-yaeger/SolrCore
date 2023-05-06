namespace SolrCore.QueryBuilder
{
    using System.Collections.Generic;
    using SolrCore.Query;

    public class FL : Query
    {
        private readonly FieldsBase _fields;
        private readonly IList<IQuery> _queries = new List<IQuery>();

        public FL(params IQuery[] queries)
        {
            foreach (var query in queries)
            {
                if (_fields == null && query is FieldsBase fieldsBase)
                {
                    _fields = fieldsBase;
                }
                else
                {
                    _queries.Add(query);
                }
            }
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

            if (_queries != null && _queries.Count > 0)
            {
                dto.Sb.Append(',');
                for (var i = 0; i < _queries.Count; i++)
                {
                    _queries[i].Build(dto);
                    if (i != _queries.Count - 1)
                    {
                        dto.Sb.Append(',');
                    }
                }
            }
        }
    }
}