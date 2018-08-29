using System.Collections.Generic;
using Newtonsoft.Json;

namespace Citybike
{
    class BikeRentalStation
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("bikesAvailable")]
        public int BikesAvailable { get; set; }

        [JsonProperty("spacesAvailable")]
        public int SpacesAvailable { get; set; }

        [JsonProperty("allowDropoff")]
        public bool AllowDropoff { get; set; }

        [JsonProperty("isFloatingBike")]
        public bool IsFloatingBike { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("realTimeData")]
        public bool RealTimeData { get; set; }
    }

    class BikeRentalStationList
    {
        [JsonProperty("stations")]
        public List<BikeRentalStation> Stations { get; set; }

        public BikeRentalStation getStationByName(string name)
        {
            foreach (BikeRentalStation station in Stations)
            {
                if (station.Name == name)
                {
                    return station;
                }
            }

            return null;
        }
    }
}