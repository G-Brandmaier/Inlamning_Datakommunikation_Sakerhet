namespace Client_Console.Models;

public class WeatherData
{
    public string Forecast { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
    public int Temperature { get; set; }
    public int MaxTemperature { get; set; }
    public int MinTemperature { get; set; }

    public WeatherData()
    {
        Forecast = "Cloudy with 20% chance of rain";
        City = "Stockholm";
        Country = "Sweden";
        Temperature = GenerateTemperature();
        MaxTemperature = 16;
        MinTemperature = 11;
    }

    public int GenerateTemperature()
    {
        Random rnd = new Random();
        var tempNow = rnd.Next(12, 15);
        return tempNow;
    }
}