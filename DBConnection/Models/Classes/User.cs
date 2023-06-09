using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DBConnection.Models.Classes
{
    public class User
    {
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

        [ForeignKey("Role")]
        public int RoleId { get; set; } = 2;
        public Role? Role { get; set; }

        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
    }
}

//[Column("customer_id")]
//[ForeignKey("Customer")]
//public int CustomerId { get; set; }
//public Customer? Customer { get; set; }