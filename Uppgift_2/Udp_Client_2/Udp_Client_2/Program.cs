using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Udp_Client_Listener;

internal class Program
{
    static void Main(string[] args)
    {
        //When program starts call the listener method
        StartListener();
    }

    //Method to start the listener
    private static void StartListener()
    {
        //Sets up listener to listen on port 8080
        int port = 8080;
        UdpClient udpClientListener = new UdpClient(port);

        //Gets which ip address that connects on selected port
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);

        try
        {
            while(true)
            {
                //Writes out message that we are waiting for a connection to broadcast the data we recive
                Console.WriteLine("Waiting to broadcast...\n");

                //Recieves the data from any incoming ip address on the selected port
                byte[] bytes = udpClientListener.Receive(ref ipEndPoint);

                //Converting the bytes that we received to a string
                string data = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                //Writes out the data we recieved
                Console.WriteLine($"Recieved data from IP {ipEndPoint}:");
                Console.WriteLine($"{data}\n");
            }
        }
        catch (Exception ex)
        {
            //Writes out the error message
            Console.WriteLine("Something went wrong");
            Console.WriteLine(ex.Message);

        }
        finally 
        { 
            //Closes listener
            udpClientListener.Close(); 
        }
    }
}