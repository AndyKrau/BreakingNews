using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBConnection.Models.Classes
{
    [Index("Id", "Title")]
    [Table("articles", Schema = "public")]
    public class Article
    {
        // делаем поле id автоинкрементным
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("source")]
        public string? Source { get; set; }

        [Column("author")]
        public string? Author { get; set; }

        [Column("title")]
        public string? Title { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("url")]
        public string? Url { get; set; }

        [Column("url_to_image")]
        public string? UrlToImage { get; set; }

        [Column("published_at")]
        public DateTime? PublishedAt { get; set; }

        [Column("content")]
        public string? Content { get; set; }
    }
}