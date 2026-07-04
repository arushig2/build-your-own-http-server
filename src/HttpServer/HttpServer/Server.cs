using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpServer
{
    public class Server
    {

        public void Start()
        {
            const int Port = 8080;
            var ipEndPoint = new IPEndPoint(IPAddress.Any, Port);
            TcpListener listener = new(ipEndPoint);
            Console.WriteLine("Starting server...");
            try
            {
                
                listener.Start();
                Console.WriteLine($"Listening on port {Port}...");
                Console.WriteLine("Waiting for a client to connect...");

                var client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected.");

                using NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, 1024);

                string dataRead = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Data recieved : {dataRead}");



            }
            finally
            {
                listener.Stop();
                Console.WriteLine($"Stopped listening on port {Port}...");
            }
        }
    }
}
