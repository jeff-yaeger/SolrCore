namespace SolrCore.Connection
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class SolrConnection : ISolrConnection
    {
        private readonly IHttpClientFactory _factory;

        public SolrConnection(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> PostAsync(string serialize, string coreName, bool commit = true)
        {
            if (string.IsNullOrEmpty(coreName))
            {
                throw new ArgumentNullException("Solr core name is not registered. Please add core name");
            }

            if (string.IsNullOrEmpty(serialize))
            {
                return false;
            }

#if DEBUG
            Console.WriteLine($"Request: {serialize}");
#endif

            var json = new StringContent(
                serialize,
                Encoding.UTF8,
                "application/json");

            var httpClient = GetClient();
            var requestUri = $"solr/{coreName}/update?commit={commit.ToSolrValue()}";
            using (var response =
                   await httpClient.PostAsync(requestUri, json))
            {
#if DEBUG
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    Console.WriteLine($"Reason: {response.ReasonPhrase}");
                    Console.WriteLine($"Content: {await response.Content.ReadAsStringAsync()}");
                }
#endif
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<string> GetAsync(string query, string coreName)
        {
            if (string.IsNullOrEmpty(coreName))
            {
                throw new ArgumentNullException("Solr core name is not registered. Please add core name");
            }

            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException("Query cannot be null");
            }

            var httpClient = GetClient();

            var requestUri = $"solr/{coreName}/select{query}";
            using (var response =
                   await httpClient.GetAsync(requestUri))
            {
#if DEBUG
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    Console.WriteLine($"Reason: {response.ReasonPhrase}");
                    Console.WriteLine($"Content: {await response.Content.ReadAsStringAsync()}");
                }
#endif
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;
            }
        }

        private HttpClient GetClient()
        {
            return _factory.CreateClient("SolrCore");
        }
    }
}