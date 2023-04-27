namespace SolrCoreWeb.Util
{
    public static class EnumerableUtil
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> values)
        {
            return values == null || values.Count == 0;
        }
    }
}