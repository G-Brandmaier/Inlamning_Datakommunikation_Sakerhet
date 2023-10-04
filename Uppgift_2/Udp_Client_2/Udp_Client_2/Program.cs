using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Udp_Client_Listener;

internal class Program
{
    static void Main(string[] args)
    {
        StartListener();
    }

    /// <summary>
    /// Setting up udp listener on selected port.
    /// Listener awaits data to be received.
    /// When data is recevied it's converted from bytes to string and then writes to the console from which ip the data comes from and the data.
    /// Then the listener awaits data to be recevied again.
    /// </summary>
    private static void StartListener()
    {
        int port = 8080;
        UdpClient udpClientListener = new UdpClient(port);
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);

        try
        {
            while(true)
            {
                Console.WriteLine("Waiting to broadcast...\n");

                byte[] bytes = udpClientListener.Receive(ref ipEndPoint);

                string data = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                Console.WriteLine($"Recieved data from IP {ipEndPoint}:");
                Console.WriteLine($"{data}\n");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Something went wrong");
            Console.WriteLine(ex.Message);
        }
        finally 
        { 
            udpClientListener.Close(); 
        }
    }
}