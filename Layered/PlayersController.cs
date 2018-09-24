using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Layered.Controllers
{
    [Route("api/[controller]")]
    public class PlayersController : Controller
    {
        private readonly PlayersProcessor _processor;

        public PlayersController(PlayersProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet]
        [Route("{id}")]
        public Task<Player> Get(Guid id)
        {
            return _processor.Get(id);
        }

        [HttpGet]
        public Task<Player[]> GetAll()
        {
            return _processor.GetAll();
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

        [HttpDelete]
        [Route("{id}")]
        public Task<Player> Delete(Guid id)
        {
            return _processor.Delete(id);
        }
    }
}