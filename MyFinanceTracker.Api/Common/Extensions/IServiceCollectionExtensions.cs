using Microsoft.OpenApi;

namespace MyFinanceTracker.Api.Common.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My Finance Tracker API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme. Enter your token below."
                });
                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MyFinanceTracker.Api.xml"));
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "MyFinanceTracker.Application.xml"));
                options.EnableAnnotations();
            });
            return services;
        }
    }
}
