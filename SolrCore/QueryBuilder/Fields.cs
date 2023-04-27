namespace SolrCore.QueryBuilder
{
    using System;

    public class Fields : Query
    {
        private readonly string[] _names;

        public Fields(params string[] names)
        {
            _names = names ?? throw new ArgumentNullException(nameof(names));
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            for (var i = 0; i < _names.Length; i++)
            {
                _names[i] = GetFieldName(_names[i], dto.Translations);
            }

            dto.Sb.Append($"{string.Join(",", _names)}");
        }
    }
}