namespace SolrCore.QueryBuilder
{
    using SolrCore.Query;

    public class Child : Query
    {
        private readonly IQuery[] _queries;

        public Child(params IQuery[] queries)
        {
            _queries = queries;
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            dto.Sb.Append("[child");
            if (_queries != null && _queries.Length > 0)
            {
                dto.Sb.Append(' ');
                for (var i = 0; i < _queries.Length; i++)
                {
                    _queries[i].Build(dto);
                    if (i != _queries.Length - 1)
                    {
                        dto.Sb.Append(' ');
                    }
                }
            }

            dto.Sb.Append(']');
        }
    }
}