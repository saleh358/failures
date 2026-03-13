using Failures.Application;
using Failures.Application.Features.Failures.Queries;
using Failures.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader();
        }
    );
});

// Add Layers
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Minimal API Endpoints
var apiConfig = app.MapGroup("/api/v1/dashboard");

apiConfig
    .MapGet(
        "/failures",
        async (IMediator mediator, int? count) =>
        {
            var query = new GetRecentFailuresQuery { Count = count ?? 10 };
            var result = await mediator.Send(query);
            return Results.Ok(result);
        }
    )
    .WithName("GetRecentFailures");

app.Run();
