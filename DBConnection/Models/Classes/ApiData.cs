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


        // Пересмотреть метод неправильные переменные
        public string GetUrl()
        {
            var country = Country?.CountryKey;
            var resultUrl = Url + $"apiKey={ApiKey}";

            if (country != null)
            {
                resultUrl = Url + $"country={country}&" + $"apiKey={ApiKey}";
            }

            return resultUrl;
        } 

    }
}
