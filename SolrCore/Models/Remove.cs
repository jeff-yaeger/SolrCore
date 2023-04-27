namespace SolrCore.Models
{
    public class Remove : IUpdate
    {
        public Remove(object value)
        {
            Value = value;
        }

        [SolrFieldName("remove")]
        public object Value { get; }
    }
}