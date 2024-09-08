using TrackingService.Models;

namespace TrackingService.Repositories;

public interface IClientRepository
{
    public Task<VehiclePoint?> GetLastPoint(string busId);
}