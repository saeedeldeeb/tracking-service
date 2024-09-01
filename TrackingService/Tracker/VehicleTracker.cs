using TrackingService.Repositories;

namespace TrackingService.Tracker;

public class VehicleTracker
{
    private readonly ITrackingRepository _trackingRepository;
    
    public VehicleTracker(ITrackingRepository trackingRepository)
    {
        _trackingRepository = trackingRepository;
    }
    
    public void Run()
    {
        while (_trackingRepository.VehicleExists("bus1"))
        {
            
        }
    }
}