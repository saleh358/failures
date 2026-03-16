using System.Reflection;
using Failures.Domain.IService;
using Microsoft.Extensions.DependencyInjection;

namespace Failures.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );
        services.AddScoped<IFailureService, FailureService>();

        return services;
    }
}
