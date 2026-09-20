using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Seed;

namespace Shared.Data
{
    public static class Extensions
    {
        public static async Task<IApplicationBuilder> UseMigrationAsync<TContext>(this IApplicationBuilder app)
            where TContext : DbContext
        {
            // 1. إنشاء Scope واحد غير تزامني (AsyncScope) لكلا العمليتين
            await using var scope = app.ApplicationServices.CreateAsyncScope();

            // 2. تطبيق الـ Migration
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            await context.Database.MigrateAsync();

            // 3. تطبيق الـ Seeding
            var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
            foreach (var seeder in seeders)
            {
                await seeder.SeedAllAsync();
            }

            return app;
        }
    }
}
