using TrackingService.Tracker;

namespace TrackingService.Handlers;

public class TripStartedHandler
{
   private readonly ILogger<TripStartedHandler> _logger;
   private readonly VehicleTracker _vehicleTracker;
   
   public TripStartedHandler(ILogger<TripStartedHandler> logger, VehicleTracker vehicleTracker)
   {
      _logger = logger;
      _vehicleTracker = vehicleTracker;
   }
   public Task HandleAsync(TripStarted tripStarted)
   {
      _logger.LogInformation("Trip started: {TripId}", tripStarted.TripId);
      
      var thread = new Thread(_vehicleTracker.Run);
      thread.Start();
      
      return Task.CompletedTask;
   }
}