using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnection.Models.Classes
{
    [Index("Id")]
    [Table("countries", Schema = "public")]
    public class Country
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("country_name")]
        public string? CountryName { get; set; }

        [Column("country_key")]
        public string? CountryKey { get; set; }


        // ссылка на строчку подключения
        public List<ApiData> Data { get; set; }
    }
}
