namespace Client_App.Models;

public class WeatherDto
{
    public string Forecast { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Temperature { get; set; }
    public int MaxTemperature { get; set; }
    public int MinTemperature { get; set; }
}
