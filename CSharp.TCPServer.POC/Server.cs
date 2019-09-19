/*
 * Filename: Server.cs
 * Description: Main code file of the NETGame.CSharp.Server.Core project
 * Project: Server code
*/

// Adding dependencies
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using System.IO;

namespace CSharp.TCPServer.POC
{
    class Server
    {
        // Will listen for TCP Connexion
        TcpListener server = null;

        // Global Variables
        IPAddress _ipAddr;
        int _port;
        string serverNameInConsole = "Server CORE";

        // Constructor
        public Server(string ip, int port)
        {
            _ipAddr = IPAddress.Parse(ip);
            _port = port;

            server = new TcpListener(_ipAddr, _port);
            server.Start();

            printLog(serverNameInConsole, "server successfully started!");

            // Method called by the thread forever
            StartListener();
        }

        // Method called when you want to print something in the console
        private void printLog(string entity, string message)
        {
            Console.WriteLine(entity + "> " + message);
        }

        // Code called all the time by the thread, dedicated by client
        public void StartListener()
        {
            try
            {
                // Infinite loop while a client is connected
                while (true)
                {
                    printLog(serverNameInConsole, "Waiting for clients...");

                    // Create a TCPClient when a client will try to connect to the server
                    TcpClient client = server.AcceptTcpClient();

                    printLog(serverNameInConsole, "new client connected! Initializing thread...");

                    // New thread by client
                    Thread t = new Thread(new ParameterizedThreadStart(HandleDevice));
                    t.Start(client);
                    printLog(serverNameInConsole, "new thread successfully created!");
                }
            }

            catch (Exception ex)
            {
                printLog(serverNameInConsole, "Error while initializing new thread, error: " + ex.Message);
            }
        }

        // Method used by each thread
        public void HandleDevice(Object obj)
        {
            // Local variables
            var threadID = Thread.CurrentThread.ManagedThreadId;
            var clientNameInConsole = "Client n°" + threadID;

            try
            {
                printLog(clientNameInConsole, "thread initialized");

                TcpClient client = (TcpClient)obj;

                // Reading incoming data from client
                var stream = client.GetStream();
                Byte[] bytes = new Byte[102400];
                StringBuilder myCompleteMessage = new StringBuilder();
                int nbrOfBytesRead = 0;

                do
                {
                    do
                    {
                        nbrOfBytesRead = stream.Read(bytes, 0, bytes.Length);
                        myCompleteMessage.AppendFormat("{0}", Encoding.UTF8.GetString(bytes, 0, nbrOfBytesRead));
                    }
                    while (stream.DataAvailable);

                    // Flushing data stream
                    stream.Flush();

                    string str = threadID.ToString();
                    Byte[] reply = Encoding.UTF8.GetBytes(str);
                    stream.Write(reply, 0, reply.Length);

                    // Printing string received by client
                    printLog(clientNameInConsole, myCompleteMessage.ToString());

                    // Resetting byte array (data received by client)
                    bytes = new Byte[102400];

                    // Resetting StringBuilder
                    myCompleteMessage.Clear();
                } while (true);
            }
            catch (Exception ex)
            {
                printLog(serverNameInConsole, "client n°" + threadID + " disconnected! Reason: " + ex.Message);
                Thread.CurrentThread.Interrupt();
            }
        }
    }
}