using Newtonsoft.Json;
using StackExchange.Redis;
using TrackingService.Models;

namespace TrackingService.Repositories;

public class TrackingRepository : ITrackingRepository
{
    private readonly IConnectionMultiplexer _redis;

    public TrackingRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public void AppendVehiclePoint(string busId, Vehicle vehicle)
    {
        var value = JsonConvert.SerializeObject(vehicle);
        _redis.GetDatabase().ListRightPush($"vehicle:{busId}:points", value);
    }

    public void AppendVehicle(string busId)
    {
        var unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        _redis.GetDatabase().SortedSetAdd("vehicles", busId, unixTime);
        _redis.GetDatabase().StringSet($"vehicle:{busId}:exists", true);
    }

    public bool VehicleExists(string busId)
    {
        return bool.Parse(_redis.GetDatabase().StringGet($"vehicle:{busId}:exists").ToString());
    }

    public Vehicle? GetLastPoint(string busId)
    {
        var value = _redis.GetDatabase().ListGetByIndex($"vehicle:{busId}:points", -1);
        return value.IsNull ? null : JsonConvert.DeserializeObject<Vehicle>(value!);
    }
}