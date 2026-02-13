using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Infrastructure.Data;
using MyFinanceTracker.Infrastructure.Identity;
using MyFinanceTracker.Infrastructure.Common.Extensions;

namespace MyFinanceTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register ApplicationDbContext with PostgreSQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity();

            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}
