using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace puppeteer_backend.Controllers
{
    public class CommandRequestDto {
        public string id;
        public List<string> commands;

    }

    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        public CommandController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get(string id)
        {
            var puppets = this.memoryCache.Get("puppets") as HashSet<string>;
            if (puppets == null) {
                puppets = new HashSet<string>();
                this.memoryCache.Set("puppets", puppets);
            }
            puppets.Add(id);

            var puppetCommands = this.memoryCache.Get($"commands_{id}") as Queue<string>;
            if (puppetCommands == null) {
                return new List<string>();
            }
            var commands = puppetCommands.ToList();
            puppetCommands.Clear();
            return commands;
        }

        [HttpPost]
        public ActionResult Post([FromBody] CommandRequestDto puppet)
        {
            var puppetCommands = this.memoryCache.Get($"commands_{puppet.id}") as Queue<string>;
            if (puppetCommands == null) {
                puppetCommands = new Queue<string>();
                this.memoryCache.Set($"commands_{puppet.id}", puppetCommands);
            }
            foreach (var command in puppet.commands)
            {
                puppetCommands.Enqueue(command);
            }
            return Ok();
        }

    }
}
