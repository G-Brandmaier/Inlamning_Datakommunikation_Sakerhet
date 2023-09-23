using System;
using System.Net.Sockets;
using System.Text;

namespace Server_Side;

internal class Program
{
    static void Main(string[] args)
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
                Console.WriteLine("Connected to client!");

                NetworkStream stream = client.GetStream();
                StreamWriter sw = new StreamWriter(stream);

                try
                {
                    byte[] buffer = new byte[256];
                    stream.Read(buffer, 0, buffer.Length);

                    string request = Encoding.ASCII.GetString(buffer, 0 , buffer.Length);

                    Console.WriteLine($"Recived information from client: {request}\n");

                    sw.WriteLine("The information was recived successfully on the server");
                    sw.Flush();
                }
                catch(Exception ex) 
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
