namespace TrackingService.Handlers;

public class TripStarted
{
    public Guid TripId { get; set; }
    public Guid BusId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string StartLocation { get; set; } = string.Empty;
    public string EndLocation { get; set; } = string.Empty;
}
