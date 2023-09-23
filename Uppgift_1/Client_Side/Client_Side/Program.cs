using Client_Side.Classes;
using System.Net.Sockets;
using System.Text.Json;

namespace Client_Side;

internal class Program
{
    static void Main(string[] args)
    {
        //New instance of the weather class to set up the data we want to send
        Weather weather = new ("Cloudy and 50% chance of rain", 16, 10, DateTime.Now.Date.ToShortDateString(), DateTime.Now.ToShortTimeString());

        //Converting the object to a json string
        string jsonStringToSend = JsonSerializer.Serialize(weather);

        //Calling the method with the data
        Connect(jsonStringToSend);
    }

    //Method for sending data using tcp client
    static void Connect(string productInformation)
    {
        try
        {
            //Setting up which port we want to use
            var port = 8080;

            //Setting up our tcp client to connect with the localhost ip address on the selected port, i'm using a using so it will self close
            using TcpClient client = new TcpClient("127.0.0.1", port);

            //Converting the json string to a byte array 
            byte[] data = System.Text.Encoding.ASCII.GetBytes(productInformation);

            //Setting up the stream for the client
            NetworkStream stream = client.GetStream();

            //Writing/sending the data we converted on the stream to the connected server
            stream.Write(data, 0, data.Length);

            //Message for the user that the data is sent
            Console.WriteLine($"Sent to server: {productInformation}\n");

            //Setting a new byte array
            byte[] buffer = new byte[256];

            //Reading the incomming data on the strem
            stream.Read(buffer, 0, buffer.Length);

            //Converting the response from the server from bytes to string
            string response = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);

            //Writing out a message for the user what we received from the server
            Console.WriteLine($"Received from server: {response}\n");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            //Writes out a message if there is an error
            Console.WriteLine("Failed to connect");
            Console.WriteLine(ex.Message);
        }
    }
}