using RabbitMQ.Client;
using TrackingService;
using TrackingService.Handlers;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddHostedService<TripConsumer>()
    .AddTransient<TripStartedHandler>()
    .AddSingleton(new ConnectionFactory
    {
        Uri = builder.Configuration.GetSection("RabbitMq:Uri").Get<Uri>(),
        DispatchConsumersAsync = true
    });

var host = builder.Build();
host.Run();