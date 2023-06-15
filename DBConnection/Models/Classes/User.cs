using Microsoft.AspNetCore.Identity;

namespace DBConnection.Models.Classes
{
    public class User : IdentityUser
    {
        public string? PostalCode { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
