namespace SolrCore.Models
{
    public class AddDistinct : IUpdate
    {
        public AddDistinct(object value)
        {
            Value = value;
        }

        [SolrFieldName("add-distinct")]
        public object Value { get; }
    }
}