using AutoMapper;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SM.Application.Common.Behaviors;
using System.Reflection;

namespace SM.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
        public static IServiceCollectionBusConfigurator AddApplication(this IServiceCollectionBusConfigurator services)
        {
            services.AddConsumers(Assembly.GetExecutingAssembly());
            services.AddActivities(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
