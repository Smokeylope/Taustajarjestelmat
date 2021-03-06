using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Layered
{
    public class MongoDbRepository : IRepository
    {
        private readonly IMongoCollection<Player> collection;
        private readonly IMongoCollection<LogEntry> logCollection;
        private readonly IMongoCollection<BsonDocument> bsonDocumentCollection;

        public MongoDbRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = mongoClient.GetDatabase("game");
            collection = database.GetCollection<Player>("players");
            logCollection = database.GetCollection<LogEntry>("log");
            bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
        }

        public Task<Player> GetPlayer(Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq("_id", id);
            return collection.Find(filter).FirstAsync();
        }

        public async Task<Player[]> GetAllPlayers()
        {
            List<Player> players = await collection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player> CreatePlayer(Player player)
        {
            await collection.InsertOneAsync(player);
            return player;
        }

        public async Task<Player> ModifyPlayer(Guid id, ModifiedPlayer player)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq("_id", id);
            UpdateDefinition<Player> update = Builders<Player>.Update.Set("Score", player.Score)
                                                                     .Set("Level", player.Level);
            await collection.UpdateOneAsync(filter, update);
            Player result = await collection.Find(filter).FirstAsync();
            return result;
        }

        public async Task<Player> DeletePlayer(Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq("_id", id);
            Player player = await collection.Find(filter).FirstAsync();
            await collection.DeleteOneAsync(filter);
            return player; 
        }

        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            Player player = await GetPlayer(playerId);
            
            foreach (Item item in player.Items)
            {
                if (item.Id == itemId)
                {
                    return item;
                }
            }

            return null;
        }

        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            Player player = await GetPlayer(playerId);
            return player.Items.ToArray();
        }

        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq("_id", playerId);
            var update = Builders<Player>.Update.Push("Items", item);
            await collection.FindOneAndUpdateAsync(filter, update);
            return item;
        }

        public async Task<Item> ModifyItem(Guid playerId, Guid itemId, ModifiedItem modifiedItem)
        {
            Item[] items = await GetAllItems(playerId);

            foreach (Item item in items)
            {
                if (item.Id == itemId)
                {
                    item.Type = modifiedItem.Type;

                    FilterDefinition<Player> filter = Builders<Player>.Filter.Eq("_id", playerId);
                    var update = Builders<Player>.Update.Set("Items", items.ToList());
                    await collection.FindOneAndUpdateAsync(filter, update);
                    
                    return item;
                }
            }

            return null;
        }

        public async Task<Item> DeleteItem(Guid playerId, Guid itemId)
        {
            Item item = await GetItem(playerId, itemId);

            if (item != null)
            {
                FilterDefinition<Player> playerFilter = Builders<Player>.Filter.Eq("_id", playerId);
                FilterDefinition<Item> itemFilter = Builders<Item>.Filter.Eq("_id", itemId);
                var update = Builders<Player>.Update.PullFilter("Items", itemFilter);
                await collection.FindOneAndUpdateAsync(playerFilter, update);
            }
            
            return item;
        }

        public async Task<int> GetPlayerLevel(Guid playerId)
        {
            Player player = await GetPlayer(playerId);
            return player.Level;
        }

        public async Task<Player[]> GetPlayersByMinScore(int score)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Gt("Score", score);
            List<Player> players = await collection.Find(filter).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player> GetPlayerByName(string name)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq("Name", name);
            Player player = await collection.Find(filter).FirstAsync();
            return player;
        }

        public async Task<Player[]> GetPlayersByItemType(ItemType type)
        {
            FilterDefinition<Item> itemFilter = Builders<Item>.Filter.Eq("Type", type);
            FilterDefinition<Player> filter = Builders<Player>.Filter.ElemMatch("Items", itemFilter);
            List<Player> players = await collection.Find(filter).ToListAsync();
            return players.ToArray();
        }

        public async Task<int> GetMostCommonPlayerLevel()
        {
            var aggregate = collection.Aggregate()
                                      .Project(e => new {Level = e.Level})
                                      .Group(e => e.Level, g => new {Level = g.Key, Count = g.Sum(o=>1)})
                                      .SortByDescending(e=> e.Count)
                                      .Limit(3);
            var result = await aggregate.FirstAsync();
            return result.Level;
        }

        public async Task AuditDeleteStarted()
        {
            await logCollection.InsertOneAsync(new LogEntry("A request to delete player started at " + DateTime.Now.ToString()));
        }

        public async Task AuditDeleteSuccess()
        {
            await logCollection.InsertOneAsync(new LogEntry("A request to delete player ended at " + DateTime.Now.ToString()));
        }
    }
}