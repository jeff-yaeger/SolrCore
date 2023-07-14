namespace SolrCoreWeb.Controllers
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using SolrCore.Models;
    using SolrCore.Query;
    using SolrCore.QueryBuilder;
    using SolrCore.Repository;

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    public class OtherTestAuthenticatedController : ControllerBase
    {
        private readonly ILogger<OtherTestAuthenticatedController> _logger;
        private readonly ISolrRepository<string, OtherItem> _solrRepository;

        public OtherTestAuthenticatedController(ILogger<OtherTestAuthenticatedController> logger,
            ISolrRepository<string, OtherItem> solrRepository)
        {
            _logger = logger;
            _solrRepository = solrRepository;
        }

        [HttpPost]
        public async Task<bool> Post(OtherItem item)
        {
            var res = await _solrRepository.AddAsync(item);
            return res;
        }

        [HttpPost("Range")]
        public async Task<bool> PostRange(IEnumerable<OtherItem> items)
        {
            var res = await _solrRepository.AddRangeAsync(items);
            return res;
        }

        [HttpPut]
        public async Task<bool> Put(OtherItem item)
        {
            var update = Map(item);
            var res = await _solrRepository.UpdateAsync(update);
            return res;
        }

        [HttpPut("Range")]
        public async Task<bool> PutRange(IEnumerable<OtherItem> items)
        {
            var updates = items.Select(Map);
            var res = await _solrRepository.UpdateRangeAsync(updates);
            return res;
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(string id)
        {
            var res = await _solrRepository.DeleteAsync<ItemUpdate>(id);
            return res;
        }

        private static ItemUpdate Map(OtherItem item)
        {
            return new ItemUpdate
            {
                Id = item.Id,
                Name = string.IsNullOrEmpty(item.Name) ? null : new Set(item.Name),
                Type = item.Type.HasValue ? new Set(item.Type) : null,
                Price = item.Price.HasValue ? new Set(item.Price) : null
            };
        }
    }
}