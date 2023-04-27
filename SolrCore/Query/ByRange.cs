namespace SolrCore.Query
{
    using System;
    using QueryBuilder;

    public class ByRange : Query
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

            _name = name;
            _start = start;
            _end = end;
        }

        public override string Render(Builder dto)
        {
            return $"{GetFieldName(_name, dto.Translations)}:[{_start} TO {_end}]";
        }
    }
}