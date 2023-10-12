﻿using Client_Console.Services;

namespace Client_Console;

internal class Program
{
    ///// <summary>
    ///// Make an instance of HubconnectionService to setup the SignalR connection and 
    ///// starts the connection setup
    ///// </summary>
    static void Main(string[] args)
    {
        HubConnectionService hubConnectionService = new HubConnectionService();
        hubConnectionService.Start().Wait();
    }
}