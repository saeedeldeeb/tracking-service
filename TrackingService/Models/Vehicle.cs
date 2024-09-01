namespace TrackingService.Models;

public class Vehicle
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}