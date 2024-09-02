using TrackingService.Handlers;
using TrackingService.Repositories;

namespace TrackingService.Tracker;

public class VehicleTracker
{
    private readonly ITrackingRepository _trackingRepository;
    
    public VehicleTracker(ITrackingRepository trackingRepository)
    {
        _trackingRepository = trackingRepository;
    }
    
    public void Run(object data)
    {
        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is running");

        var tripStarted = (TripStarted)data;
        while (_trackingRepository.VehicleExists(tripStarted.BusId.ToString()))
        {
            var vehicle = _trackingRepository.GetLastPoint(tripStarted.BusId.ToString());
            if (vehicle != null)
            {
                Console.WriteLine($"Vehicle: {vehicle.Latitude}, {vehicle.Longitude}");
            }

            Thread.Sleep(1000);   
        }
    }
}