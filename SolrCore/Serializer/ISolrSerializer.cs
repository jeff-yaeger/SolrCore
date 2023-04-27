namespace SolrCore.Serializer
{
    public interface ISolrSerializer
    {
        string Serialize<TValue>(TValue value);
        TValue Deserialize<TValue>(string json);
    }
}