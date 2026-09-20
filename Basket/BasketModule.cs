using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Basket
{
    public static class BasketModule
    {
        public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
        {
            // Register services related to the Basket module
            // Configure options if needed
            return services;
        }
        public async static Task<IApplicationBuilder>  UseBasketModuleAsync(this IApplicationBuilder app)
        {
            // Configure middleware related to the Basket module if needed
            return app;
        }
    }
}
