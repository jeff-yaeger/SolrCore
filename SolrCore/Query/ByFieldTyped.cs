namespace SolrCore.Query
{
    using System;
    using Models;
    using QueryBuilder;

    public class ByField<T> : Query, IQuote where T : SolrEntity<T>
    {
        private readonly string _name;
        private readonly object _value;
        private bool _quotes;

        public ByField(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(name);
            }

            _name = GetFieldName(name, SolrEntity<T>.Translations);
            _value = value;
        }

        public void AddQuotes()
        {
            _quotes = true;
        }

        public override string Render(Builder dto)
        {
            return _quotes ? $"{_name}:\"{_value}\"" : $"{_name}:{_value}";
        }
    }
}