using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TrackingService.Handlers;
using TrackingService.RabbitMQ.Bases;

namespace TrackingService;

public sealed class TripConsumer : ConsumerBase, IHostedService
{
    protected override string QueueName => LoggerQueue;

    public TripConsumer(
        ConnectionFactory connectionFactory,
        ILogger<TripConsumer> tripConsumerLogger,
        ILogger<ConsumerBase> consumerLogger,
        ILogger<RabbitMqClientBase> logger,
        TripStartedHandler handler
    ) : base(connectionFactory, consumerLogger, logger)
    {
        try
        {
            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += async (sender, @event) =>
            {
                var message = OnEventReceived<TripStarted>(sender, @event);
                if (message is not null)
                {
                    await handler.HandleAsync(message);
                }
            };
            Channel.BasicConsume(
                queue: QueueName,
                autoAck: false,
                consumer: consumer
            );
        }
        catch (Exception e)
        {
            tripConsumerLogger.LogCritical(e, "Error while consuming message from queue.");
        }
    }

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Dispose();
        return Task.CompletedTask;
    }
}