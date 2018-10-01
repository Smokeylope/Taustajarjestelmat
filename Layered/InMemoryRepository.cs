using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Layered
{
    public class InMemoryRepository : IRepository
    {
        private Dictionary<Guid, Player> Players;
        private Dictionary<Guid, Dictionary<Guid, Item>> Items;

        public InMemoryRepository()
        {
            Players = new Dictionary<Guid, Player>();
            Items = new Dictionary<Guid, Dictionary<Guid, Item>>();
        }

        Task<Player> IRepository.GetPlayer(Guid id)
        {
            Console.WriteLine("GET");
            Player player = null;
            Players.TryGetValue(id, out player);
            return Task.FromResult(player);
        }

        Task<Player[]> IRepository.GetAllPlayers()
        {
            Console.WriteLine("GETALL");
            Console.WriteLine(Players.Count);
            List<Player> playerList = new List<Player>();

            foreach (KeyValuePair<Guid, Player> pair in Players)
            {
                playerList.Add(pair.Value);
                Console.WriteLine(pair.Value);
            }

            return Task.FromResult(playerList.ToArray());
        }

        Task<Player> IRepository.CreatePlayer(Player player)
        {
            Console.WriteLine("CREATE");
            Players.Add(player.Id, player);
            Console.WriteLine(Players.Count);
            return Task.FromResult(player);
        }

        Task<Player> IRepository.ModifyPlayer(Guid id, ModifiedPlayer player)
        {
            Player p = null;
            Players.TryGetValue(id, out p);

            if (p != null)
            {
                p.Score = player.Score;
                p.Level = player.Level;
                return Task.FromResult(p);
            }

            return Task.FromResult((Player) null);
        }

        Task<Player> IRepository.DeletePlayer(Guid id)
        {
            Player p = null;
            Players.TryGetValue(id, out p);

            if (p != null)
            {
                Players.Remove(id);
                return Task.FromResult(p);
            }

            return Task.FromResult((Player) null);
        }

        Task<Item> IRepository.GetItem(Guid playerId, Guid itemId)
        {
            Dictionary<Guid, Item> playerItems = null;
            Items.TryGetValue(playerId, out playerItems);

            if (playerItems != null)
            {
                Item item = null;
                playerItems.TryGetValue(itemId, out item);
                return Task.FromResult(item);
            }

            return Task.FromResult((Item) null);
        }

        Task<Item[]> IRepository.GetAllItems(Guid playerId)
        {
            Dictionary<Guid, Item> playerItems = null;
            Items.TryGetValue(playerId, out playerItems);

            if (playerItems != null)
            {
                return Task.FromResult(playerItems.Values.ToArray());
            }
            
            return Task.FromResult(new Item[0]);
        }

        Task<Item> IRepository.CreateItem(Guid playerId, Item item)
        {
            if (!Items.ContainsKey(playerId))
            {
                Items.Add(playerId, new Dictionary<Guid, Item>());
            }

            Items[playerId].Add(item.Id, item);
            return Task.FromResult(item);
        }

        Task<Item> IRepository.ModifyItem(Guid playerId, Guid itemId, ModifiedItem item)
        {
            if (Items.ContainsKey(playerId))
            {
                if (Items[playerId].ContainsKey(itemId))
                {
                    Items[playerId][itemId].Type = item.Type;
                    return Task.FromResult(Items[playerId][itemId]);
                }
            }

            return Task.FromResult((Item) null);
        }

        Task<Item> IRepository.DeleteItem(Guid playerId, Guid itemId)
        {
            if (Items.ContainsKey(playerId))
            {
                if (Items[playerId].ContainsKey(itemId))
                {
                    Item item = Items[playerId][itemId];
                    Items[playerId].Remove(itemId);
                    return Task.FromResult(item);
                }
            }

            return Task.FromResult((Item) null);
        }

        Task<int> IRepository.GetPlayerLevel(Guid playerId)
        {
            return Task.FromResult(Players[playerId].Level);
        }

        public Task<Player[]> GetPlayersByMinScore(int score)
        {
            throw new NotImplementedException();
        }

        public Task<Player> GetPlayerByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetPlayersByItemType(ItemType type)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMostCommonPlayerLevel()
        {
            throw new NotImplementedException();
        }
    }
}