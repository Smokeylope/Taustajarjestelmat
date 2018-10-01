using System;
using System.Threading.Tasks;

namespace Layered
{
    public interface IRepository
    {
        Task<Player> GetPlayer(Guid id);
        Task<Player[]> GetAllPlayers();
        Task<Player> CreatePlayer(Player player);
        Task<Player> ModifyPlayer(Guid id, ModifiedPlayer player);
        Task<Player> DeletePlayer(Guid id);

        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> CreateItem(Guid playerId, Item item);
        Task<Item> ModifyItem(Guid playerId, Guid itemId, ModifiedItem item);
        Task<Item> DeleteItem(Guid playerId, Guid itemId);

        Task<int> GetPlayerLevel(Guid playerId);

        Task<Player[]> GetPlayersByMinScore(int score);
        Task<Player> GetPlayerByName(string name);
        Task<Player[]> GetPlayersByItemType(ItemType type);
        Task<int> GetMostCommonPlayerLevel();
    }
}