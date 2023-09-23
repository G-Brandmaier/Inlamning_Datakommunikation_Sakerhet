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
        //Setting up our weather data that we want to send
        WeatherData weather = new("Cloudy and 50% chance of rain", 16, 10, DateTime.Now.Date.ToShortDateString(), DateTime.Now.ToShortTimeString());

        //Converting the object to a json string
        string jsonStringToSend = JsonSerializer.Serialize(weather);

        //Calling the method to send the data
        SendData(jsonStringToSend);
    }

    //Method to send data over udp
    private static void SendData(string data)
    {
        //Create a instance of our udp client
        UdpClient udpClient = new UdpClient();
        try
        {
            //Set up the connection on the localhost ip address on port 8080
            udpClient.Connect("127.0.0.1", 8080);

            //Converting the json string to bytes
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            //Sending the byte array through our udp client 
            udpClient.Send(bytes, bytes.Length);

            //Closing the client
            udpClient.Close();

            //Message for the user that the message was sent
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