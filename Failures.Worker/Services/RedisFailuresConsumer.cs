using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Failures.Application.Interfaces;
using Failures.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Failures.Worker.Services;

public class RedisFailuresConsumer(
    IConnectionMultiplexer _redis,
    IServiceScopeFactory _scopeFactory,
    ILogger<RedisFailuresConsumer> _logger
) : BackgroundService
{
    private const string StreamName = "PaymentFailedEvent";
    private const string GroupName = "notification_group";
    private const string ConsumerName = "notification_consumer_1";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var db = _redis.GetDatabase();

        try
        {
            if (
                !await db.KeyExistsAsync(StreamName)
                || (await db.StreamGroupInfoAsync(StreamName)).All(g => g.Name != GroupName)
            )
            {
                await db.StreamCreateConsumerGroupAsync(StreamName, GroupName, "0-0", true);
                _logger.LogInformation(
                    "Created consumer group {GroupName} for stream {StreamName}",
                    GroupName,
                    StreamName
                );
            }
        }
        catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
        {
            // Group already exists
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await db.StreamReadGroupAsync(
                    StreamName,
                    GroupName,
                    ConsumerName,
                    ">",
                    count: 1
                );

                if (result.Length > 0)
                {
                    foreach (var message in result)
                    {
                        var payload = message.Values[0].Value;
                        _logger.LogInformation(
                            "RedisFailuresConsumer sending email for failed payment. Payload: {Payload}",
                            payload
                        );

                        if (!payload.IsNullOrEmpty)
                        {
                            try
                            {
                                var failedPayment = JsonSerializer.Deserialize<FailedPayment>(
                                    payload.ToString()
                                );
                                if (failedPayment != null)
                                {
                                    using var scope = _scopeFactory.CreateScope();
                                    var dbContext =
                                        scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                                    dbContext.FailedPayments.Add(failedPayment);
                                    await dbContext.SaveChangesAsync(stoppingToken);
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(
                                    ex,
                                    "Error deserializing or saving failed payment. Payload: {Payload}",
                                    payload
                                );
                            }
                        }

                        await db.StreamAcknowledgeAsync(StreamName, GroupName, message.Id);
                    }
                }
                else
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing stream in RedisFailuresConsumer");
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
