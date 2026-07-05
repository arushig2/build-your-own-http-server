using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

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

                
                string requestLine = dataRead.Split("\n")[0];
                string route = requestLine.Split(" ")[1];


                string body = "Invalid Route";

                if (route == "/")
                {
                    body = "Home Page";
                }
                else if (route == "/about")
                {
                    body = "About Page";
                }
                else if (route == "/contact")
                {
                    body = "Contact us at example@email.com";
                }

                int numBytes = Encoding.UTF8.GetByteCount(body);

                string response = $"""
                                  HTTP/1.1 200 OK
                                  Content-Type: text/html; charset=utf-8
                                  Content-Length: {numBytes}

                                  {body}
                                  """;

               byte[] responseBytes = Encoding.UTF8.GetBytes(response);
               stream.Write(responseBytes, 0, responseBytes.Length);

            }
            finally
            {
                listener.Stop();
                Console.WriteLine($"Stopped listening on port {Port}...");
            }
        }
    }
}
