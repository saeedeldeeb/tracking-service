using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace TrackingService.RabbitMQ.Bases;

public abstract class ProducerBase<T>(
    ConnectionFactory connectionFactory,
    ILogger<RabbitMqClientBase> logger,
    ILogger<ProducerBase<T>> producerBaseLogger)
    : RabbitMqClientBase(connectionFactory, logger), IRabbitMqProducer<T>
{
    protected abstract string ExchangeName { get; }
    protected abstract string RoutingKeyName { get; }
    protected abstract string AppId { get; }

    public virtual void Publish(T @event)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
            var properties = Channel?.CreateBasicProperties();
            if (properties == null)
                return;
            properties.AppId = AppId;
            properties.ContentType = "application/json";
            properties.DeliveryMode = 1; // Doesn't persist to disk
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            Channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: RoutingKeyName,
                body: body,
                basicProperties: properties
            );
        }
        catch (Exception ex)
        {
            producerBaseLogger.LogCritical(ex, "Error while publishing");
        }
    }
}
