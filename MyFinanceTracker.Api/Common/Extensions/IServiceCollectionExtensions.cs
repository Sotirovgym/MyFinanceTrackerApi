namespace MyFinanceTracker.Api.Common.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
                {
                    Title = "My Finance Tracker API",
                    Version = "v1"
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MyFinanceTracker.Api.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MyFinanceTracker.Application.xml"));
                options.EnableAnnotations();
            });
            return services;
        }
    }
}
