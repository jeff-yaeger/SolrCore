namespace SolrCore.QueryBuilder
{
    using Models;

    public class Sort<T> : Query where T : SolrEntity<T>
    {
        private readonly string _name;
        private readonly string _sortOrder;

        public Sort(string name, SortEnum sortEnum = SortEnum.Asc)
        {
            _name = GetFieldName(name, SolrEntity<T>.Translations);
            _sortOrder = sortEnum == SortEnum.Desc ? "desc" : "asc";
        }

        public override string Render(Builder dto)
        {
            return $"sort={_name} {_sortOrder}";
        }
    }
}