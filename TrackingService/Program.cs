using RabbitMQ.Client;
using StackExchange.Redis;
using TrackingService;
using TrackingService.Handlers;
using TrackingService.Helpers;
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
    .AddTransient<IClientRepository, ClientRepository>()
    .AddTransient<ITrackingRepository, TrackingRepository>()
    .AddTransient<JsonSerializerOptionsWrapper>()
    .AddTransient<VehicleTracker>()
    .AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var uri = builder.Configuration.GetSection("Redis:Uri").Value;
        if (uri == null)
            throw new InvalidOperationException("Redis URI is not set");

        var configuration = ConfigurationOptions.Parse(uri);
        return ConnectionMultiplexer.Connect(configuration);
    }).AddHttpClient("VehiclesAPIClient",
        configureClient =>
        {
            configureClient.BaseAddress = new Uri("https://1a35da32-e5b7-4aae-99e3-3f6622cbe4a3.mock.pstmn.io");
            configureClient.Timeout = new TimeSpan(0, 0, 30);
        });

var host = builder.Build();
host.Run();