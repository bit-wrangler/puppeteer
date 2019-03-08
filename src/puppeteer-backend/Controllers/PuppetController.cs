using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace puppeteer_backend.Controllers
{
    public class PuppetDto {
        public string id;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PuppetController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        public PuppetController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var puppets = this.memoryCache.Get("puppets") as HashSet<string>;
            if (puppets != null) {
                return puppets.ToList();
            }

            return new List<string>();
        }

        public static void AddPuppet(string id, IMemoryCache memCache)
        {
            var puppets = memCache.Get("puppets") as HashSet<string>;
            if (puppets == null) {
                puppets = new HashSet<string>();
            }
            puppets.Add(id);
        }

    }
}
