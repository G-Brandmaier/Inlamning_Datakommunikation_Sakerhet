using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Udp_Client.Classes;

namespace Udp_Client;

internal class Program
{
    //Setting up the data  I want to send and converting it to a json string then calling the SendData method. 
    static void Main(string[] args)
    {
        WeatherData weather = new("Cloudy and 50% chance of rain", 16, 10, DateTime.Now.Date.ToShortDateString(), DateTime.Now.ToShortTimeString());
        string jsonStringToSend = JsonSerializer.Serialize(weather);

        SendData(jsonStringToSend);
    }

    /// <summary>
    /// Setting up a udp client.
    /// Convertng the json string to bytes and then sending the data to a server that listens on localhost and selected port.
    /// When data is sent the client closes.
    /// </summary>
    /// <param name="data"></param>
    private static void SendData(string data)
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
            //Message for the user that there was an error
            Console.WriteLine("Something went wrong!");
            Console.WriteLine(ex.Message);

        }
    }
}