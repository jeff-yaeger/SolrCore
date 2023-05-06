namespace SolrCore.QueryBuilder
{
    using System;

    public class Fields : FieldsBase
    {
        public Fields(params string[] names)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }

        public override void Build(Builder dto)
        {
            for (var i = 0; i < Names.Length; i++)
            {
                Names[i] = GetFieldName(Names[i], dto.Translations);
            }

            dto.Sb.Append($"{string.Join(",", Names)}");
        }
    }
}