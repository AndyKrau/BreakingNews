using Microsoft.AspNetCore.Identity;

namespace DBConnection.Models.Classes
{
    public class User : IdentityUser
    {
        public DateOnly? DateOfBirth { get; set; }
        public string? PostalCode { get; set; } = string.Empty;

        public string? Country { get; set; } = string.Empty;

        public string? City { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
    }
}
