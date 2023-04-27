namespace SolrCore.Models
{
    using System;
    using System.Text.Json.Serialization;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class SolrFieldNameAttribute : JsonAttribute
    {
        public SolrFieldNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}