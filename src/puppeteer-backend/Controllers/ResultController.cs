using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace puppeteer_backend.Controllers
{
    public class ResultRequestDto {
        public string id;
        public List<string> results;

    }

    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        public ResultController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(string id)
        {
            var puppetResults = this.memoryCache.Get($"results_{id}") as Queue<string>;
            if (puppetResults == null) {
                return new List<string>();
            }
            var results = puppetResults.ToList();
            puppetResults.Clear();
            return results;
        }

        [HttpPost]
        public ActionResult Post([FromBody] ResultRequestDto puppet)
        {
            var puppetResults = this.memoryCache.Get($"results_{puppet.id}") as Queue<string>;
            if (puppetResults == null) {
                puppetResults = new Queue<string>();
                this.memoryCache.Set($"results_{puppet.id}", puppetResults);
            }
            foreach (var result in puppet.results)
            {
                puppetResults.Enqueue(result);
            }
            return Ok();
        }

    }
}
