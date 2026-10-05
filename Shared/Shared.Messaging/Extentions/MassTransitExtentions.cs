using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.Extentions
{
    public static class MassTransitExtentions
    {
        public static IServiceCollection AddMassTransitWithAssmblies(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
                config.SetInMemorySagaRepositoryProvider();

                config.AddConsumers(assemblies);
                config.AddSagaStateMachines(assemblies);
                config.AddSagas(assemblies);
                config.AddActivities(assemblies);

                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["MessageBroker:Host"], host =>
                    {
                        host.Username(configuration["MessageBroker:Username"]!);
                        host.Password(configuration["MessageBroker:Password"]!);
                    });
                    cfg.ConfigureEndpoints(context);    
                });
            });
            return services;
        }
    }
}
