using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Citybike
{
    public class OfflineCityBikeDataFetcher : ICityBikeDataFetcher
    {
        Task<int> ICityBikeDataFetcher.GetBikeCountInStation(string stationName)
        {
            if (stationName.Any(c => char.IsDigit(c)))
            {
                throw new System.ArgumentException();
            }
            
            string[] lines = System.IO.File.ReadAllLines("bikedata.txt");

            foreach (string line in lines)
            {
                string[] substrings = line.Split(" : ");

                if (substrings.Length == 2 && substrings[0] == stationName)
                {
                    return Task.FromResult(Int32.Parse(substrings[1]));
                }
            }

            throw new NotFoundException();
        }
    }
}