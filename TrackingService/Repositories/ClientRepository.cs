using System.Text.Json;
using TrackingService.Helpers;
using TrackingService.Models;

namespace TrackingService.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptionsWrapper _jsonSerializerOptionsWrapper;


    public ClientRepository(IHttpClientFactory httpClientFactory,
        JsonSerializerOptionsWrapper jsonSerializerOptionsWrapper)
    {
        _httpClientFactory = httpClientFactory;
        _jsonSerializerOptionsWrapper = jsonSerializerOptionsWrapper;
    }

    public async Task<VehiclePoint?> GetLastPoint(string busId)
    {
        var httpClient = _httpClientFactory.CreateClient("VehiclesAPIClient");
        var response = await httpClient.GetAsync($"vehicles/busid/last-point");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<VehiclePoint>(content, _jsonSerializerOptionsWrapper.Options);
    }
}