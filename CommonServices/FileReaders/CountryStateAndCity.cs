using CommonServices.Model.City;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CommonServices.FileReaders
{
    public static class CountryStateAndCity
    {
        public static List<Country> GetStateAndCity()
        {
            string json = FileReader.ReadJsonFromFile("ms-appx:///Assets/CountryStateCity.json");
            return JsonConvert.DeserializeObject<List<Country>>(json);
        }
    }
}
