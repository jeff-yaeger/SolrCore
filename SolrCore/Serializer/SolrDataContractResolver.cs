namespace SolrCore.Serializer
{
    using System.Linq;
    using System.Reflection;
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class SolrDataContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (property.AttributeProvider?.GetAttributes(typeof(SolrFieldNameAttribute), true).FirstOrDefault() is SolrFieldNameAttribute attribute)
            {
                property.PropertyName = attribute.Name;
            }

            return property;
        }
    }
}