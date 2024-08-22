namespace TrackingService.Handlers;

public class TripStartedHandler
{
   private readonly ILogger<TripStartedHandler> _logger;
   
   public TripStartedHandler(ILogger<TripStartedHandler> logger)
   {
      _logger = logger;
   }
   public Task HandleAsync(TripStarted tripStarted)
   {
      _logger.LogInformation("Trip started: {TripId}", tripStarted.TripId);
      return Task.CompletedTask;
   }
}