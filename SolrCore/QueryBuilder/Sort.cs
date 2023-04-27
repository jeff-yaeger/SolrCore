namespace SolrCore.QueryBuilder
{
    public class Sort : Query
    {
        private readonly string _name;
        private readonly string _sortOrder;

        public Sort(string name, SortEnum sortEnum = SortEnum.Asc)
        {
            _name = name;
            _sortOrder = sortEnum == SortEnum.Desc ? "desc" : "asc";
        }

        public override string Render(Builder dto)
        {
            return $"sort={GetFieldName(_name, dto.Translations)} {_sortOrder}";
        }
    }
}