using Client_Side.Classes;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Xml.Linq;

namespace Client_Side;

internal class Program
{
    static void Main(string[] args)
    {

        Weather weather = new ("Cloudy and 50% chance of rain", 16, 10, DateTime.Now.Date.ToShortDateString(), DateTime.Now.ToShortTimeString());

        string jsonStringToSend = JsonSerializer.Serialize(weather);

        Connect(jsonStringToSend);
    }

    static void Connect(string productInformation)
    {
        try
        {
            var port = 8080;
            using TcpClient client = new TcpClient("127.0.0.1", port);

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(productInformation);

            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);

            Console.WriteLine($"Sent to server: {productInformation}\n");

            data = new byte[256];

            string response = string.Empty;

            var bytes = stream.Read(data, 0, data.Length);
            response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine($"Received from server: {response}\n");

            Console.ReadKey();

        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to connect");
            Console.WriteLine(ex.Message);
        }
    }
}