namespace SolrCore.Models
{
    public class Increment : IUpdate
    {
        public Increment(object value)
        {
            Value = value;
        }

        [SolrFieldName("inc")]
        public object Value { get; }
    }
}