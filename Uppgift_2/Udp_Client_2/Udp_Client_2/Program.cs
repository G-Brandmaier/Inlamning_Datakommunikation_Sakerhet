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
            Console.WriteLine("No connection was found");
            Console.WriteLine(ex.Message);

        }
        finally { udpClientListener.Close(); }
    }
}