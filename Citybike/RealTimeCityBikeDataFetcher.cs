using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;

namespace Citybike
{
    class RealTimeCityBikeDataFetcher : ICityBikeDataFetcher
    {
        async Task<int> ICityBikeDataFetcher.GetBikeCountInStation(string stationName)
        {
            if (stationName.Any(c => char.IsDigit(c)))
            {
                throw new System.ArgumentException();
            }

            System.Net.Http.HttpClient Client = new System.Net.Http.HttpClient();
            string data = await Client.GetStringAsync("http://api.digitransit.fi/routing/v1/routers/hsl/bike_rental");

            BikeRentalStationList list = JsonConvert.DeserializeObject<BikeRentalStationList>(data);
            BikeRentalStation station = list.getStationByName(stationName);

            if (station == null)
            {
                throw new NotFoundException();
            }

            return station.BikesAvailable;
        }
    }
}