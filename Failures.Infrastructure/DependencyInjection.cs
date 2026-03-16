using Failures.Application.Interfaces;
using Failures.Domain.Abstraction;
using Failures.Domain.IService;
using Failures.Infrastructure.Persistence;
using Failures.Infrastructure.Repository;
using Failures.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Failures.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Entity Framework Core setup
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>()
        );

        // Redis setup
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(
                configuration.GetConnectionString("Redis") ?? "localhost:6379"
            )
        );
        services.AddScoped<IFailureService, FailureService>();
        services.AddScoped<IFailureRepository, FailureRepository>();

        // Background Service
        return services;
    }
}
