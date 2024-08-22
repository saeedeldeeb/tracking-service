namespace TrackingService.RabbitMQ.Bases;

public interface IRabbitMqProducer<in T>
{
    void Publish(T @event);
}
