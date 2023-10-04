using Client_Side.Classes;
using System.Net.Sockets;
using System.Text.Json;

namespace Client_Side;

internal class Program
{
    // Setting up the object I want to send and converting it to a jsonstring then call the connect method.
    static void Main(string[] args)
    {
        Weather weather = new ("Cloudy and 50% chance of rain", 16, 10, DateTime.Now.Date.ToShortDateString(), DateTime.Now.ToShortTimeString());
        string jsonStringToSend = JsonSerializer.Serialize(weather);
        Connect(jsonStringToSend);
    }

    /// <summary>
    /// Method to connect to server on the local ip and selected port number.
    /// Converting the json string to bytes and sending the data to the server.
    /// Then receiving the response from the server when the sent data is received at the server.
    /// </summary>
    /// <param name="productInformation"></param>
    static void Connect(string productInformation)
    {
        try
        {
            var port = 8080;
            using TcpClient client = new TcpClient("127.0.0.1", port);

            byte[] data = System.Text.Encoding.ASCII.GetBytes(productInformation);

            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);

            Console.WriteLine($"Sent to server: {productInformation}\n");

            byte[] buffer = new byte[256];
            stream.Read(buffer, 0, buffer.Length);
            string response = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);

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