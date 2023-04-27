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
    public class TestAuthenticatedController : ControllerBase
    {
        private readonly ILogger<TestAuthenticatedController> _logger;
        private readonly ISolrRepository<string, Item> _solrRepository;

        public TestAuthenticatedController(ILogger<TestAuthenticatedController> logger,
            ISolrRepository<string, Item> solrRepository)
        {
            _logger = logger;
            _solrRepository = solrRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new QueryBuilder(
                new Q(new ByRange(nameof(Item.CreatedDate), "NOW-2DAYS", "NOW").And(
                    new ByField(nameof(Item.Name), "Popsicles").And(
                        new ByField(nameof(Item.Price), 5.0)).Boost(1.3).Or(
                        new ByField(nameof(Item.Name), "Bicycles").And(
                            new ByField(nameof(Item.Type), 3).Not())))
                ),
                new FL(
                    new Fields("id", nameof(Item.Name), nameof(Item.Price))
                ),
                new Sort(nameof(Item.CreatedDate), SortEnum.Desc),
                new Start(),
                new Rows(10)
            );

            var res = await _solrRepository.GetAsync(query);
            return Ok(res);
        }

        [HttpPost]
        public async Task<bool> Post(Item item)
        {
            var res = await _solrRepository.AddAsync(item);
            return res;
        }

        [HttpPost("Range")]
        public async Task<bool> PostRange(IEnumerable<Item> items)
        {
            var res = await _solrRepository.AddRangeAsync(items);
            return res;
        }

        [HttpPut]
        public async Task<bool> Put(Item item)
        {
            var update = Map(item);
            var res = await _solrRepository.UpdateAsync(update);
            return res;
        }

        [HttpPut("Range")]
        public async Task<bool> PutRange(IEnumerable<Item> items)
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

        private static ItemUpdate Map(Item item)
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