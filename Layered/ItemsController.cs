using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Layered.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ItemsProcessor _processor;

        public ItemsController(ItemsProcessor processor)
        {
            _processor = processor;
        }

        [HttpGet]
        [Route("api/players/{playerId}/items/{itemId}")]
        public Task<Item> Get(Guid playerId, Guid itemId)
        {
            return _processor.Get(playerId, itemId);
        }

        [HttpGet]
        [Route("api/players/{playerId}/items")]
        public Task<Item[]> GetAll(Guid playerId)
        {
            return _processor.GetAll(playerId);
        }

        [HttpPost]
        [Route("api/players/{playerId}/items")]
        [ValidateModel]
        [LevelTooLowExceptionFilter]
        public Task<Item> Create(Guid playerId, NewItem item)
        {
            return _processor.Create(playerId, item);
        }

        [HttpPut]
        [Route("api/players/{playerId}/items/{itemId}")]
        [ValidateModel]
        public Task<Item> Modify(Guid playerId, Guid itemId, ModifiedItem item)
        {
            return _processor.Modify(playerId, itemId, item);
        }

        [HttpDelete]
        [Route("api/players/{playerId}/items/{itemId}")]
        public Task<Item> Delete(Guid playerId, Guid itemId)
        {
            return _processor.Delete(playerId, itemId);
        }
    }
}