namespace SolrCore.QueryBuilder
{
    using System;

    public abstract class FieldsBase : Query
    {
        protected string[] Names;

        protected FieldsBase(params string[] names)
        {
            Names = names ?? throw new ArgumentNullException(nameof(names));
        }

        public override string Render(Builder dto)
        {
            return Render(Build, dto);
        }
    }
}