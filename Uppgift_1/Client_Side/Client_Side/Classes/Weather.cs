using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Side.Classes;

//Setting up the class for the data we want to send in our tcp client
public class Weather
{
    public Weather(string forecast, int maxTemperature, int minTemperature, string date, string time)
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
