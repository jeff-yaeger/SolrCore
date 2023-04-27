namespace SolrCore.Serializer
{
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization.Metadata;
    using Models;

    public class SolrSerializer : ISolrSerializer
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { ChangeToSolrName }
            }
        };

        public string Serialize<TValue>(TValue value)
        {
            return JsonSerializer.Serialize(value, _options);
        }

        public TValue Deserialize<TValue>(string json)
        {
            return JsonSerializer.Deserialize<TValue>(json, _options);
        }

        private static void ChangeToSolrName(JsonTypeInfo typeInfo)
        {
            foreach (var propertyInfo in typeInfo.Properties)
            {
                if (propertyInfo.AttributeProvider?.GetCustomAttributes(typeof(SolrFieldNameAttribute), true).FirstOrDefault() is SolrFieldNameAttribute attribute)
                {
                    propertyInfo.Name = attribute.Name;
                }
            }
        }
    }
}