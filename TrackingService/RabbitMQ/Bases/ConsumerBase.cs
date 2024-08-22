using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TrackingService.RabbitMQ.Bases;

public abstract class ConsumerBase(
    ConnectionFactory connectionFactory,
    ILogger<ConsumerBase> consumerLogger,
    ILogger<RabbitMqClientBase> logger)
    : RabbitMqClientBase(connectionFactory, logger)
{
    protected abstract string QueueName { get; }

    protected virtual T? OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
    {
        T? message = default;
        try
        {
            var body = Encoding.UTF8.GetString(@event.Body.ToArray());
            message = JsonConvert.DeserializeObject<T>(body);
        }
        catch (Exception ex)
        {
            consumerLogger.LogCritical(ex, "Error while retrieving message from queue.");
        }
        finally
        {
            Channel?.BasicAck(@event.DeliveryTag, false);
        }

        return message;
    }
}
