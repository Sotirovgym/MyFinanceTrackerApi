using Microsoft.AspNetCore.Identity;

namespace MyFinanceTracker.Infrastructure.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool EnableNotifications { get; set; }
    }
}
