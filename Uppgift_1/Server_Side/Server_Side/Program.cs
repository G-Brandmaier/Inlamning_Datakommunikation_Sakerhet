using System.Net.Sockets;
using System.Text;

namespace Server_Side;

internal class Program
{
    //Calling the StartListener method
    static void Main(string[] args)
    {
        StartListener();
    }

    /// <summary>
    /// Setting up tcp listener on local ip and selected port number.
    /// The listener is waiting for client to connect.
    /// When connection has been made the data received is read and a response message is sent back to the connected client.
    /// Then the listener waits for a new connection to be made.
    /// </summary>
    private static void StartListener()
    {
        try
        {
            var port = 8080;
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, port);

            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for connection...\n");

                using TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Connected to client!\n");

                NetworkStream stream = client.GetStream();
                try
                {
                    byte[] buffer = new byte[256];
                    stream.Read(buffer, 0, buffer.Length);

                    string request = Encoding.ASCII.GetString(buffer, 0, buffer.Length);

                    Console.WriteLine($"Recived information from client: {request}\n");

                    stream.Write(Encoding.ASCII.GetBytes("The information was recived successfully on the server"));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to recive data");
                    Console.WriteLine(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("No connection was found");
            Console.WriteLine(ex.Message);
        }
    }
}
