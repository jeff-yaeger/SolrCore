namespace SolrCore.Models
{
    public class Add : IUpdate
    {
        public Add(object value)
        {
            Value = value;
        }

        [SolrFieldName("add")]
        public object Value { get; }
    }
}