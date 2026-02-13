using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyFinanceTracker.Infrastructure.Data;
using MyFinanceTracker.Infrastructure.Identity;

namespace MyFinanceTracker.Infrastructure.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = true;
            });
        }
    }
}
