namespace SolrCore.Models
{
    using Newtonsoft.Json;

    public class Set : IUpdate
    {
        public Set(object value)
        {
            Value = value;
        }

        [SolrFieldName("set")]
        [JsonProperty]
        public object Value { get; }
    }
}