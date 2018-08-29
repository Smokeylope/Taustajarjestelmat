using System.Threading.Tasks;

namespace Citybike
{
    public interface ICityBikeDataFetcher
    {
        Task<int> GetBikeCountInStation(string stationName);
    }
}