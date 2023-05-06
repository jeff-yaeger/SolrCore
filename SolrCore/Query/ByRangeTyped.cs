namespace SolrCore.Query
{
    using System;
    using Models;
    using QueryBuilder;

    public class ByRange<T> : Query where T : SolrEntity<T>
    {
        private readonly string _end;
        private readonly string _name;
        private readonly string _start;

        public ByRange(string name, string start, string end)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(name);
            }

            _name = GetFieldName(name, SolrEntity<T>.Translations);
            _start = start;
            _end = end;
        }

        public override string Render(Builder dto)
        {
            return $"{_name}:[{_start} TO {_end}]";
        }
    }
}