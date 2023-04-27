namespace SolrCore.Serializer
{
    using Newtonsoft.Json;

    public class SolrNewtonsoftJsonSerializer : ISolrSerializer
    {
        private readonly JsonSerializerSettings _options;

        public SolrNewtonsoftJsonSerializer()
        {
            _options = new JsonSerializerSettings
            {
                ContractResolver = new SolrDataContractResolver(),
                DateFormatString = Constants.SolrDateFormat
            };
        }

        public string Serialize<TValue>(TValue value)
        {
            return JsonConvert.SerializeObject(value, _options);
        }

        public TValue Deserialize<TValue>(string json)
        {
            return JsonConvert.DeserializeObject<TValue>(json, _options);
        }
    }
}