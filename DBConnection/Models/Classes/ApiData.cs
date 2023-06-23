using DBConnection.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBConnection.Models.Classes
{
    [Index("Id")]
    [Table("api_data", Schema = "public")]
    public class ApiData
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("api_key")]
        public string? ApiKey { get; set; }

        [Column("url")]
        public string? Url { get; set; }

        [Column("country_id")]
        public int CountryId { get; set; }
        public Country Country { get; set; }


        public string GetUrl(ApiDataConnectionContext apiDataConnection)
        {
            var apiDb = apiDataConnection.Countries;
            var country = apiDb.FirstOrDefault(c => c.Id == CountryId);
            var countryKey = country!.CountryKey;

            var resultUrl = Url + $"country={countryKey}&" + $"apiKey={ApiKey}";

            return resultUrl;
        } 

    }
}
