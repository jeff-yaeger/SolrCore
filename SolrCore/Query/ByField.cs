namespace SolrCore.Query
{
    using System;
    using QueryBuilder;

    public class ByField : Query, IQuote
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

            _name = name;
            _value = value;
        }

        public void AddQuotes()
        {
            _quotes = true;
        }

        public override string Render(Builder dto)
        {
            return _quotes ? $"{GetFieldName(_name, dto.Translations)}:\"{_value}\"" : $"{GetFieldName(_name, dto.Translations)}:{_value}";
        }
    }
}