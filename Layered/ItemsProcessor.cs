using System;
using System.Threading.Tasks;

namespace Layered
{
    public class ItemsProcessor
    {
        private readonly IRepository _repository;

        public ItemsProcessor(IRepository repository)
        {
            _repository = repository;
        }

        public Task<Item> Get(Guid playerId, Guid itemId)
        {
            return _repository.GetItem(playerId, itemId);
        }

        public Task<Item[]> GetAll(Guid playerId)
        {
            return _repository.GetAllItems(playerId);
        }

        public Task<Item> Create(Guid playerId, NewItem item)
        {
            if (item.Type == ItemType.Sword && _repository.GetPlayerLevel(playerId).Result < 3)
            {
                throw new LevelTooLowException("Item type sword requires player level 3 or higher.");
            }

            Item newItem = new Item();
            newItem.Id = Guid.NewGuid();
            newItem.Level = item.Level;
            newItem.Type = item.Type;
            newItem.CreationTime = DateTime.Now;
            return _repository.CreateItem(playerId, newItem);
        }

        public Task<Item> Modify(Guid playerId, Guid itemId, ModifiedItem item)
        {
            return _repository.ModifyItem(playerId, itemId, item);
        }

        public Task<Item> Delete(Guid playerId, Guid itemId)
        {
            return _repository.DeleteItem(playerId, itemId);
        }
    }
}