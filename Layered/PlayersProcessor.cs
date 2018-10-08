using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Layered
{
    public class PlayersProcessor
    {
        private readonly IRepository _repository;

        public PlayersProcessor(IRepository repository)
        {
            _repository = repository;
        }

        public Task<Player> Get(Guid id)
        {
            return _repository.GetPlayer(id);
        }

        public Task<Player> GetPlayerByName(string name)
        {
            return _repository.GetPlayerByName(name);
        }

        public Task<Player[]> GetAll()
        {
            return _repository.GetAllPlayers();
        }

        public Task<Player[]> GetPlayersByMinScore(int minScore)
        {
            return _repository.GetPlayersByMinScore(minScore);
        }

        public Task<Player[]> GetPlayersByItemType(ItemType type)
        {
            return _repository.GetPlayersByItemType(type);
        }

        public Task<int> GetMostCommonPlayerLevel()
        {
            return _repository.GetMostCommonPlayerLevel();
        }

        public Task<Player> Create(NewPlayer player)
        {
            Player newPlayer = new Player();
            newPlayer.Id = Guid.NewGuid();
            newPlayer.Name = player.Name;
            newPlayer.CreationTime = DateTime.Now;
            return _repository.CreatePlayer(newPlayer);
        }

        public Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            return _repository.ModifyPlayer(id, player);
        }
        
        public Task<Player> Delete(Guid id)
        {
            return _repository.DeletePlayer(id);
        }
    }
}