using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extentions
{
    public static class CarterExtentions
    {
        public static IServiceCollection AddCarterWithAssmblies(this IServiceCollection services, params Assembly[] assemblies)
        {
                services.AddCarter(configurator :config =>
                {
                    foreach (var assembly in assemblies)
                    {
                        var modules = assembly.GetTypes().Where(t => typeof(ICarterModule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract).Distinct().ToArray();
                        config.WithModules(modules);
                    }

                });
            return services;
        }
    }
}
