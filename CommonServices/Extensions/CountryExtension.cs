using CommonServices.Model.City;
using Newtonsoft.Json;
using System.Collections.Generic;
using DbModel = CommonServices.Model.DbModel;

namespace CommonServices.Extensions
{
    public static class CountryExtension
    {
        public static DbModel.Country ToDbCountry(this Country country)
        {
            DbModel.Country dbCountry = new DbModel.Country
            {
                Id = country.Id,
                Capital = country.Capital,
                Currency = country.Currency,
                CurrencySymbol = country.CurrencySymbol,
                Emoji = country.Emoji,
                EmojiU = country.EmojiU,
                Iso2 = country.Iso2,
                Iso3 = country.Iso3,
                Latitude = country.Latitude,
                Longitude = country.Longitude,
                Name = country.Name,
                Native = country.Native,
                NumericCode = country.NumericCode,
                PhoneCode = country.PhoneCode,
                Region = country.Region,
                Subregion = country.Subregion,
                Tld = country.Tld,
                StateJson = JsonConvert.SerializeObject(country.States)
            };

            return dbCountry;
        }

        public static Country ToCountry(this DbModel.Country country)
        {
            Country dbCountry = new Country
            {
                Id = country.Id,
                Capital = country.Capital,
                Currency = country.Currency,
                CurrencySymbol = country.CurrencySymbol,
                Emoji = country.Emoji,
                EmojiU = country.EmojiU,
                Iso2 = country.Iso2,
                Iso3 = country.Iso3,
                Latitude = country.Latitude,
                Longitude = country.Longitude,
                Name = country.Name,
                Native = country.Native,
                NumericCode = country.NumericCode,
                PhoneCode = country.PhoneCode,
                Region = country.Region,
                Subregion = country.Subregion,
                Tld = country.Tld,
                States = JsonConvert.DeserializeObject<List<State>>(country.StateJson)
            };

            return dbCountry;
        }
    }
}
