namespace SolrCore.Query
{
    using System;
    using QueryBuilder;

    public class ByField : Query
    {
        private readonly string _name;
        private readonly object _value;

        public ByField(string name, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(name);
            }

            _name = name;
            _value = value;
        }

        public override string Render(Builder dto)
        {
            return $"{GetFieldName(_name, dto.Translations)}:\"{_value}\"";
        }
    }
}