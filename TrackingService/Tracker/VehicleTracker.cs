using TrackingService.Handlers;
using TrackingService.Models;
using TrackingService.Repositories;

namespace TrackingService.Tracker;

public class VehicleTracker
{
    private readonly ITrackingRepository _trackingRepository;
    private readonly IClientRepository _clientRepository;
    
    public VehicleTracker(ITrackingRepository trackingRepository, IClientRepository clientRepository)
    {
        _trackingRepository = trackingRepository;
        _clientRepository = clientRepository;
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
            
            var realVehicle = _clientRepository.GetLastPoint(tripStarted.BusId.ToString()).Result;
            if (realVehicle != null)
            {
                _trackingRepository.AppendVehiclePoint(tripStarted.BusId.ToString(), new Vehicle
                {
                    Id = realVehicle.Id,
                    Name = realVehicle.Name,
                    DateTime = realVehicle.DateTime,
                    Latitude = realVehicle.Latitude,
                    Longitude = realVehicle.Longitude
                });

                if (!Equals(vehicle?.Longitude, realVehicle.Longitude) || vehicle.Latitude != realVehicle.Latitude)
                {
                    _trackingRepository.AppendVehiclePoint(tripStarted.BusId.ToString(), new Vehicle
                    {
                        Id = realVehicle.Id,
                        Name = realVehicle.Name,
                        DateTime = realVehicle.DateTime,
                        Latitude = realVehicle.Latitude,
                        Longitude = realVehicle.Longitude
                    });
                }
            }

            Thread.Sleep(TimeSpan.FromSeconds(5));   
        }
    }
}