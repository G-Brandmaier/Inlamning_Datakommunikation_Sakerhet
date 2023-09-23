using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using Udp_Client.Classes;

namespace Udp_Client;

internal class Program
{
    static void Main(string[] args)
    {
        WeatherData weather = new("Cloudy and 50% chance of rain", 16, 10, DateTime.Now.Date.ToShortDateString(), DateTime.Now.ToShortTimeString());

        string jsonStringToSend = JsonSerializer.Serialize(weather);

        SendMessage(jsonStringToSend);
    }

    private static void SendMessage(string data)
    {
        UdpClient udpClient = new UdpClient();
        try
        {
            udpClient.Connect("127.0.0.1", 8080);

            byte[] bytes = Encoding.UTF8.GetBytes(data);

            udpClient.Send(bytes, bytes.Length);

            udpClient.Close();

            Console.WriteLine("Data was sent");
            Console.ReadKey();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Something went wrong!");
            Console.WriteLine(ex.Message);

        }
    }
}