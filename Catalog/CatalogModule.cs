using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Register services related to the Catalog module
            services.AddDbContext<CatalogDbContext>((IServiceProvider s, DbContextOptionsBuilder options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database"));
                options.AddInterceptors(s.GetServices<ISaveChangesInterceptor>());
            });
            services.AddScoped<IDataSeeder, CatalogDataSeeder>();
            return services;
        }
        public static async Task<IApplicationBuilder> UseCatalogModuleAsync(this IApplicationBuilder app)
        {
            await app.UseMigrationAsync<CatalogDbContext>();
            // Configure middleware related to the Catalog module if needed
            return app;
        }
    }
}
