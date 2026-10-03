using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Seed;

namespace Shared.Data
{
    public static class Extensions
    {
        public static async Task<IApplicationBuilder> UseMigrationAsync<TContext>(this IApplicationBuilder app)
            where TContext : DbContext
        {
            await using var scope = app.ApplicationServices.CreateAsyncScope();

            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            await context.Database.MigrateAsync();

            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
            foreach (var seeder in seeders)
            {
                await seeder.SeedAllAsync();
            }

            return app;
        }
    }
}
