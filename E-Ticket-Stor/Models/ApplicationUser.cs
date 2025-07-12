using Microsoft.AspNetCore.Identity;

namespace E_Ticket_Stor.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Address { get; set; }
    }
}
