using Failures.Application;
using Failures.Infrastructure;
using Failures.Worker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHostedService<RedisFailuresConsumer>();

var host = builder.Build();
host.Run();
