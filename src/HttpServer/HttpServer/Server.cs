using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using static System.Net.WebRequestMethods;
using System.ComponentModel;

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

             

                while (true)
                {
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
                    string statusLine = "200 OK";
                    string contentType = "text/html; charset=utf-8";
                    byte[]? bodyBytes = null;
                    int numBytes = 0;



                    if (route == "/")
                    {
                        string filePath = "../../../wwwroot/index.html";

                        try
                        {
                            body = System.IO.File.ReadAllText(filePath);
                           
                        }
                        catch (FileNotFoundException)
                        {

                            body = "Index file Not Found";
                            statusLine = "404 Not Found";
                        }
                    }
                    else if (route == "/about")
                    {
                        string filePath = "../../../wwwroot/about.html";

                        try
                        {
                            body = System.IO.File.ReadAllText(filePath);
                        }
                        catch (FileNotFoundException)
                        {

                            body = "file Not Found";
                            statusLine = "404 Not Found";
                        }

                    }
                    else if (route == "/contact")
                    {
                        body = "Contact us at example@email.com";
                    }
                    else if (route == "/style.css")
                    {
                        string filePath = "../../../wwwroot/style.css";

                        try
                        {
                            body = System.IO.File.ReadAllText(filePath);
                            contentType = "text/css; charset=utf-8";
                        }
                        catch (FileNotFoundException)
                        {

                            body = "Index file Not Found";
                            statusLine = "404 Not Found";
                        }

                    }
                    else if (route == "/script.js")
                    {
                        string filePath = "../../../wwwroot/script.js";

                        try
                        {
                            body = System.IO.File.ReadAllText(filePath);
                            contentType = "application/javascript; charset=utf-8";
                        }
                        catch (FileNotFoundException)
                        {

                            body = "Index file Not Found";
                            statusLine = "404 Not Found";
                        }

                    } else if(route == "/img.png")
                    {
                        string filePath = "../../../wwwroot/img.png";

                        try
                        {
                            bodyBytes = System.IO.File.ReadAllBytes(filePath);
                            contentType = "image/png";
                            
                        }
                        catch (FileNotFoundException)
                        {

                            body = "Image Not Found";
                            statusLine = "404 Not Found";
                        }

                    }
                    else
                    {
                        statusLine = "404 Not Found";
                    }

                    if (bodyBytes != null)
                    {
                        numBytes = bodyBytes.Length;
                    }
                    else
                    {
                        numBytes = Encoding.UTF8.GetByteCount(body);
                    }

                    string response = $"""
                                  HTTP/1.1 {statusLine}
                                  Content-Type: {contentType}
                                  Content-Length: {numBytes}

                                  
                                  """;

                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                    stream.Write(responseBytes, 0, responseBytes.Length);

                    if (bodyBytes != null)
                    {
                        stream.Write(bodyBytes, 0, bodyBytes.Length);
                    }
                    else
                    {
                        byte[] textBytes = Encoding.UTF8.GetBytes(body);
                        stream.Write(textBytes, 0, textBytes.Length);
                    }



                    client.Close();
                }               

            }
            finally
            {
                listener.Stop();
                Console.WriteLine($"Stopped listening on port {Port}...");
            }
        }
    }
}
