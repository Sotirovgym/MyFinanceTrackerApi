using Microsoft.AspNetCore.Identity;

namespace MyFinanceTracker.Infrastructure.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public bool EnableNotifications { get; set; }
    }
}
