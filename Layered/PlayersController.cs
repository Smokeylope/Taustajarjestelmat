using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Layered.Controllers
{
    public class PlayerQuery
    {
        public int minScore { get; set; }
        public int itemType { get; set; } = -1;
    }

    [Route("api/[controller]")]
    public class PlayersController : Controller
    {
        private readonly PlayersProcessor _processor;

        public PlayersController(PlayersProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet]
        [Route("{id:guid}")]
        public Task<Player> Get(Guid id)
        {
            return _processor.Get(id);
        }

        [HttpGet]
        [Route("{name:alpha}")]
        public Task<Player> Get(string name)
        {
            return _processor.GetPlayerByName(name);
        }

        [HttpGet]
        public Task<Player[]> GetAll(PlayerQuery query)
        {
            if (query.minScore > 0)
            {
                return _processor.GetPlayersByMinScore(query.minScore);
            }
            else if (query.itemType >= 0)
            {
                return _processor.GetPlayersByItemType((ItemType) query.itemType);
            }

            return _processor.GetAll();
        }

        [HttpGet]
        [Route("level")]
        public Task<int> GetMostCommonPlayerLevel()
        {
            return _processor.GetMostCommonPlayerLevel();
        }

        [HttpPost]
        public Task<Player> Create(NewPlayer player)
        {
            return _processor.Create(player);
        }

        [HttpPut]
        [Route("{id}")]
        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            return _processor.Modify(id, player);
        }

        [Authorize(Policy = "AdminOnly")]
        [ServiceFilter(typeof(AuditFilter))]
        [HttpDelete]
        [Route("{id}")]
        public Task<Player> Delete(Guid id)
        {
            return _processor.Delete(id);
        }
    }
}