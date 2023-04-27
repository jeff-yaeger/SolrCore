namespace SolrCore.Query
{
    using QueryBuilder;

    public interface IQuery : IBuilder
    {
        string Render(Builder dto);
    }
}