using TrackingService.Repositories;
using TrackingService.Tracker;

namespace TrackingService.Handlers;

public class TripStartedHandler
{
    private readonly ILogger<TripStartedHandler> _logger;
    private readonly VehicleTracker _vehicleTracker;
    private readonly ITrackingRepository _trackingRepository;

    public TripStartedHandler(ILogger<TripStartedHandler> logger,
        VehicleTracker vehicleTracker,
        ITrackingRepository trackingRepository)
    {
        _logger = logger;
        _vehicleTracker = vehicleTracker;
        _trackingRepository = trackingRepository;
    }

    public Task HandleAsync(TripStarted tripStarted)
    {
        _logger.LogInformation("Trip started: {TripId}", tripStarted.TripId);
        
        _trackingRepository.AppendVehicle(tripStarted.BusId.ToString());

        var thread = new Thread(_vehicleTracker.Run);
        thread.Start(tripStarted);

        return Task.CompletedTask;
    }
}