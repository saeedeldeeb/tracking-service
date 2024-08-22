using RabbitMQ.Client;

namespace TrackingService.RabbitMQ.Bases;

public abstract class RabbitMqClientBase : IDisposable
{
    protected const string LoggerExchange = "events";
    protected const string LoggerQueue = "offduty.default.queue";
    protected const string LoggerQueueAndExchangeRoutingKey = "trip.started";

    protected IModel? Channel { get; private set; }
    private IConnection? _connection;
    private readonly ConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMqClientBase> _logger;

    protected RabbitMqClientBase(
        ConnectionFactory connectionFactory,
        ILogger<RabbitMqClientBase> logger
    )
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
        ConnectToRabbitMq();
    }

    private void ConnectToRabbitMq()
    {
        if (_connection == null || _connection.IsOpen == false)
        {
            _connection = _connectionFactory.CreateConnection();
        }

        if (Channel is { IsOpen: true })
            return;
        Channel = _connection.CreateModel();
        Channel.ExchangeDeclare(
            exchange: LoggerExchange,
            type: "topic",
            durable: true,
            autoDelete: false
        );
        Channel.QueueDeclare(
            queue: LoggerQueue,
            durable: false,
            exclusive: false,
            autoDelete: false
        );
        Channel.QueueBind(
            queue: LoggerQueue,
            exchange: LoggerExchange,
            routingKey: LoggerQueueAndExchangeRoutingKey
        );
    }

    public void Dispose()
    {
        try
        {
            Channel?.Close();
            Channel?.Dispose();
            Channel = null;

            _connection?.Close();
            _connection?.Dispose();
            _connection = null;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Cannot dispose RabbitMQ channel or connection");
        }
    }
}
