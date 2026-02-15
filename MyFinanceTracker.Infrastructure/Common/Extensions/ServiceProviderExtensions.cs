using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyFinanceTracker.Core.Constants;

namespace MyFinanceTracker.Infrastructure.Common.Extensions
{
    public static class ServiceProviderExtensions
    {
        public async static Task SeedRolesAsync(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roles = { RoleNames.Admin, RoleNames.User };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
        }
    }
}
