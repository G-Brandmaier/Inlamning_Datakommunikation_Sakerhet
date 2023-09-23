using System;
using System.Net.Sockets;
using System.Text;

namespace Server_Side;

internal class Program
{
    //Calling the listener method
    static void Main(string[] args)
    {
        StartListener();
    }

    //Method to start the tcp listener
    private static void StartListener()
    {
        try
        {
            //Setting port number
            var port = 8080;

            //Setting up the tcp listener to listen to any ipaddress on selected port
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, port);

            //Starting the listener
            listener.Start();

            while (true)
            {
                //Write out message for user that we are wating to connect
                Console.WriteLine("Waiting for connection...\n");

                //I'm using a using so it will self close in the end, setting up a tcp client and accepting the tcp client for our listener
                using TcpClient client = listener.AcceptTcpClient();

                //Writes out a message for the user that we have a connection
                Console.WriteLine("Connected to client!\n");

                //Setting up the stream for this client to receive and write data on
                NetworkStream stream = client.GetStream();

                try
                {
                    //Setting up a new byte array
                    byte[] buffer = new byte[256];

                    //Reading the incomming data from the stream
                    stream.Read(buffer, 0, buffer.Length);

                    //Converting the data from bytes to string
                    string request = Encoding.ASCII.GetString(buffer, 0, buffer.Length);

                    //Writes out to the user what data we got from the connected client
                    Console.WriteLine($"Recived information from client: {request}\n");

                    //Writes back on the stream to the client that the information was succesfully received
                    stream.Write(Encoding.ASCII.GetBytes("The information was recived successfully on the server"));
                }
                catch (Exception ex)
                {
                    //Writes a message for the user that the data could not be recived and the error message
                    Console.WriteLine("Failed to recive data");
                    Console.WriteLine(ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            //Writes a message for the user that no connection was found and the error message
            Console.WriteLine("No connection was found");
            Console.WriteLine(ex.Message);
        }
    }
}
