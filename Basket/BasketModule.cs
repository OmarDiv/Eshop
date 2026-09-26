using Basket.Data;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using Shared.Data;
using Shared.Data.Interceptors;
using Shared.Data.Seed;

namespace Basket
{
    public static class BasketModule
    {
        public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            }
            );
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventInterceptor>();
            services.AddDbContext<BasketDbContext>((IServiceProvider s, DbContextOptionsBuilder options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("Database"));
                options.AddInterceptors(s.GetServices<ISaveChangesInterceptor>());
            });
            return services;
        }
        public async static Task<IApplicationBuilder> UseBasketModuleAsync(this IApplicationBuilder app)
        {
            await app.UseMigrationAsync<BasketDbContext>();
            return app;
        }
    }
}
