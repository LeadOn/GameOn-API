/*
 * Filename: Program.cs
 * Description: Main code file of the NETGame.CSharp.Server.Core project
 * Project: .NET Game, first project of the year, Team C
*/ 

// Adding dependencies
using System;
using System.Net;
using System.Threading;

namespace CSharp.TCPServer.POC
{
    class Program
    {
        // Constructor
        static void Main(string[] args)
        {
            Console.WriteLine("Server Core - .NET Game - Team C");
            Console.WriteLine("==================================================");

            // Default IP and port
            var ipAddr = "127.0.0.1";
            var port = 32101;
            var tempString = "";

            // if some args were given at soft launch
            // First arg: IP Address of this server
            // Second arg: Server port
            // Default: 127.0.0.1 32101
            if (args.Length > 0 && args.Length == 2)
            {
                Console.WriteLine("Arg 0: " + args[0]);
                Console.WriteLine("Arg 1: " + args[1]);

                ipAddr = args[0];
                port = Int32.Parse(args[1]);

                Console.WriteLine("==================================================");
                Console.WriteLine("Selected @IP: " + IPAddress.Parse(ipAddr) + '\n');
                Console.WriteLine("Selected port: " + port + '\n');
                Console.WriteLine("Every settings are set, now launching the server...");
            }

            // If no args were given
            else
            {
                // Prompt user to type in @IP and port
                Console.WriteLine("If you want to set it to default, don't type anything, and press ENTER.");
                Console.Write("Please enter server IP Address (default: localhost): ");

                ipAddr = Console.ReadLine();
                if(String.IsNullOrWhiteSpace(ipAddr))
                {
                    ipAddr = "127.0.0.1";
                }

                Console.WriteLine("Selected @IP: " + IPAddress.Parse(ipAddr) + '\n');

                Console.Write("Please enter server port (default: 32101): ");

                tempString = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(tempString))
                {
                    port = 32101;
                }

                else
                {
                    port = Int32.Parse(tempString);
                }

                Console.WriteLine("Selected port: " + port + '\n');
                Console.WriteLine("Every settings are set, now launching the server...");
            }
            Console.WriteLine("==================================================\n");
            
            // Creating new thread containing TCPServer
            Thread t = new Thread(delegate ()
            {
                // replace the IP with your system IP Address...
                Server myserver = new Server(ipAddr, port);
            });

            // Launching thread
            t.Start();
        }
    }
}
