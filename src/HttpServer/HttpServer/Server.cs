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
                    string[] headers = requestLine.Split(" ");
                    string method = headers[0];
                    string route = headers[1];
                    
                    string body = "Invalid Route";
                    string statusLine = "200 OK";
                    string contentType = "text/html; charset=utf-8";
                    byte[]? bodyBytes = null;
                    int numBytes = 0;
                    string rootPath = "../../../wwwroot/";


                    if (method == "GET")
                    {
                        if (route == "/")
                        {
                            string filePath = rootPath + "index.html";

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
                            string filePath = rootPath + "about.html";

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
                            string filePath = rootPath + "contact.html";

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
                        else if (route == "/style.css")
                        {
                            string filePath = rootPath + "style.css";

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
                            string filePath = rootPath + "script.js";

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

                        }
                        else if (route == "/img.png")
                        {
                            string filePath = rootPath + "img.png";

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


                    }

                    else if (method == "POST")
                    {
                        if (route == "/contact")
                        {
                            string requestBody = dataRead.Split("\r\n\r\n")[1];
                            string[] keyValue = requestBody.Split('&');
                            string[] key = new string[keyValue.Length];
                            string[] value = new string[keyValue.Length];

                            for (int i = 0; i < keyValue.Length; i++)
                            {
                                string[] pair = keyValue[i].Split('=');

                                key[i] = pair[0];
                                value[i] = pair[1];
                            }

                            body = $"<h1>Form Submitted</h1>\r\n\r\n<p>{key[0]}: {value[0]}</p>\r\n\r\n<p>{key[1]}: {value[1]}</p>";
                            contentType = "text/html; charset=utf-8";
                            numBytes = Encoding.UTF8.GetByteCount(body);
                        }
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
