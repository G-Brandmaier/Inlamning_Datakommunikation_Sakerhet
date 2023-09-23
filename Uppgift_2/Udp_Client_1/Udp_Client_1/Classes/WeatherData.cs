using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Udp_Client.Classes;

//Class for the weather data that we want to set and send to the udp listener
public class WeatherData
{
    public WeatherData(string forecast, int maxTemperature, int minTemperature, string date, string time)
    {
        Forecast = forecast;
        MaxTemperature = maxTemperature;
        MinTemperature = minTemperature;
        Date = date;
        Time = time;
    }

    public string Forecast { get; set; }
    public int MaxTemperature { get; set; }
    public int MinTemperature { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
}
