using RabbitMQ.Client;
using StackExchange.Redis;
using TrackingService;
using TrackingService.Handlers;
using TrackingService.Repositories;
using TrackingService.Tracker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddHostedService<TripConsumer>()
    .AddTransient<TripStartedHandler>()
    .AddSingleton(new ConnectionFactory
    {
        Uri = builder.Configuration.GetSection("RabbitMq:Uri").Get<Uri>(),
        DispatchConsumersAsync = true
    })
    .AddTransient<ITrackingRepository, TrackingRepository>()
    .AddTransient<VehicleTracker>()
    .AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var uri = builder.Configuration.GetSection("Redis:Uri").Value;
        if (uri == null)
            throw new InvalidOperationException("Redis URI is not set");

        var configuration = ConfigurationOptions.Parse(uri);
        return ConnectionMultiplexer.Connect(configuration);
    });

var host = builder.Build();
host.Run();