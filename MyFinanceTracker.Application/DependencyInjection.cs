using Microsoft.Extensions.DependencyInjection;
using MyFinanceTracker.Application.Common.Interfaces;
using MyFinanceTracker.Application.Features.Accounts;
using MyFinanceTracker.Application.Features.Categories;
using MyFinanceTracker.Application.Features.Transactions;

namespace MyFinanceTracker.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITransactionService, TransactionService>();
            return services;
        }
    }
}
