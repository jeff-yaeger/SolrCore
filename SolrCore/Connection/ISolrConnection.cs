namespace SolrCore.Connection
{
    using System.Threading.Tasks;

    public interface ISolrConnection
    {
        Task<bool> PostAsync(string serialize, string coreName, bool commit = true);
        Task<string> GetAsync(string query, string coreName);
    }
}