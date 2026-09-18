using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;

namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Register services related to the Catalog module
            // Configure options if needed
            services.AddDbContext<CatalogDbContext>((s,options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database"));
            });
            return services;
        }
        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {
           app.UseMigration<CatalogDbContext>();
            // Configure middleware related to the Catalog module if needed
            return app;
        }
    }
}
