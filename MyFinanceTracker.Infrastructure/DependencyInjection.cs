using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Infrastructure.Data;
using MyFinanceTracker.Infrastructure.Data.Filters;
using MyFinanceTracker.Infrastructure.Data.Repositories;
using MyFinanceTracker.Infrastructure.Identity;
using MyFinanceTracker.Infrastructure.Common.Extensions;
using MyFinanceTracker.Application.Common.Options;

namespace MyFinanceTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<TransactionQueryFilter>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
