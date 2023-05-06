namespace SolrCore.QueryBuilder
{
    using System;
    using Models;

    public class Fields<T> : FieldsBase where T : SolrEntity<T>
    {
        public Fields(params string[] names)
        {
            if (names == null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            Names = new string[names.Length];
            for (var i = 0; i < names.Length; i++)
            {
                Names[i] = GetFieldName(names[i], SolrEntity<T>.Translations);
            }
        }

        public override string Render(Builder dto)
        {
            return $"{string.Join(",", Names)}";
        }
    }
}