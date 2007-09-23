using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using RCube.Networking.Server;
using System.ComponentModel.Design;
using RCube.Networking;
using NSpring.Logging;
using RCube.Networking.Server.Impl;

public class Program
{
    public static void Main(string[] args)
    {
        Thread serverThread = null;

        try
        {
            Console.WriteLine("Ragade's Cube Server");
            Console.WriteLine("Copyright (c) 2007");

            using(ServerApp svrApp = new ServerAppImpl())
            {
                svrApp.Run();
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            if (serverThread != null && serverThread.IsAlive)
                serverThread.Abort();
        }
    }
}