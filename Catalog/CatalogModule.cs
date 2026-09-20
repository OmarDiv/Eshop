using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Interceptors;
namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Register services related to the Catalog module
            services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            // Configure options if needed
            services.AddHttpContextAccessor();
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
            services.AddDbContext<CatalogDbContext>((IServiceProvider s, DbContextOptionsBuilder options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database"));
                options.AddInterceptors(s.GetServices<ISaveChangesInterceptor>());
            });
            services.AddScoped<IDataSeeder, CataglogDataSeeder>();
            return services;
        }
        public static async Task<IApplicationBuilder> UseCatalogModule(this IApplicationBuilder app)
        {
           app = await app.UseMigrationAsync<CatalogDbContext>();
            // Configure middleware related to the Catalog module if needed
            return app;
        }
    }
}
