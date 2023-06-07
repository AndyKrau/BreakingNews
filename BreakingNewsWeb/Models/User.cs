using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreakingNewsWeb.Models
{
    [Index("Name")]

    public class User
    {
        // делаем поле id автоинкрементным
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "No name provided")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The string length must be between 3 and 50 characters")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email address not specified")]
        [EmailAddress(ErrorMessage = "Incorrect address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; } = string.Empty;

        [StringLength(10, MinimumLength = 9, ErrorMessage = "The Code length must be 10 characters")]
        public string? PostalCode { get; set; } = string.Empty;

        public string? Country { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.user;

        //public DateTime CreatedDatetime { get; set; }

    }

    public enum Role
    {
        admin = 0,
        user = 1
    }
}
