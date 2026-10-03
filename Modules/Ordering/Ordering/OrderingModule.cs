using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering
{
    public static class OrderingModule
    {
        public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Register services related to the Ordering module
            // Configure options if needed
            return services;
        }
        public async static Task<IApplicationBuilder> UseOrderingModuleAsync(this IApplicationBuilder app)
        {
            // Configure middleware related to the Ordering module if needed
            return app;
        }
    }
}
