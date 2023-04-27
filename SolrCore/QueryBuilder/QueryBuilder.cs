namespace SolrCore.QueryBuilder
{
    using System.Collections.Generic;
    using SolrCore.Query;

    public class QueryBuilder : Query
    {
        private readonly FL _fields;
        private readonly IList<FQ> _filterQueries = new List<FQ>();
        private readonly Indent _indent;
        private readonly QOperator _operator;
        private readonly IList<IQuery> _queries = new List<IQuery>();
        private readonly Q _query;
        private readonly Rows _rows;
        private readonly Sort _sort;
        private readonly Start _start;

        public QueryBuilder(params IQuery[] queries)
        {
            foreach (var query in queries)
            {
                var found = false;
                switch (query)
                {
                    case Indent indent:
                        _indent = indent;
                        found = true;
                        break;
                    case Start start:
                        _start = start;
                        found = true;
                        break;
                    case Rows rows:
                        _rows = rows;
                        found = true;
                        break;
                    case Q q:
                        _query = q;
                        found = true;
                        break;
                    case FQ fq:
                        _filterQueries.Add(fq);
                        found = true;
                        break;
                    case FL field:
                        _fields = field;
                        found = true;
                        break;
                    case Sort sort:
                        _sort = sort;
                        found = true;
                        break;
                    case QOperator qOperator:
                        _operator = qOperator;
                        found = true;
                        break;
                }

                if (!found)
                {
                    _queries.Add(query);
                }
            }

            _indent = _indent ?? new Indent();
            _rows = _rows ?? new Rows();
            _query = _query ?? new Q(new AllQuery());
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            dto.Sb.Append('?');

            Add(_fields, dto);

            foreach (var filterQuery in _filterQueries)
            {
                filterQuery.Build(dto);
                dto.Sb.Append('&');
            }

            Add(_operator, dto);

            _query.Build(dto);
            dto.Sb.Append('&');

            _rows.Build(dto);
            dto.Sb.Append('&');

            Add(_sort, dto);
            Add(_start, dto);

            _indent.Build(dto);
            if (_queries.Count > 0)
            {
                dto.Sb.Append('&');
            }

            for (var i = 0; i < _queries.Count; i++)
            {
                _queries[i].Build(dto);
                if (i != _queries.Count - 1)
                {
                    dto.Sb.Append('&');
                }
            }
        }

        private static void Add(IBuilder builder, Builder dto)
        {
            if (builder == null)
            {
                return;
            }

            builder.Build(dto);
            dto.Sb.Append('&');
        }
    }
}