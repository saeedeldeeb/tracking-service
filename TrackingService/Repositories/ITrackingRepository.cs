using TrackingService.Models;

namespace TrackingService.Repositories;

public interface ITrackingRepository
{
    public void AppendVehiclePoint(string busId, Vehicle vehicle);
    public void AppendVehicle(string busId);
    public bool VehicleExists(string busId);
    public Vehicle? GetLastPoint(string busId);
}