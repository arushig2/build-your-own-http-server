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

             

                while (true)
                {

                    HttpRequest request = new HttpRequest();
                    HttpResponse response = new HttpResponse();

                    var client = listener.AcceptTcpClient();
                    Console.WriteLine("Client connected.");

                    using NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, 1024);

                    string dataRead = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Data recieved : {dataRead}");

                    string requestLine = dataRead.Split("\n")[0];
                    string[] requestParts = requestLine.Split(" ");
                    request.Method = requestParts[0];
                    request.Route = requestParts[1];
                    request.Version = requestParts[2];

                    response.Body = "Invalid Route";
                    response.StatusLine = "200 OK";
                    response.ContentType = "text/html; charset=utf-8";
                    response.BodyBytes = null;
                    int numBytes = 0;
                    string rootPath = "../../../wwwroot/";


                    if (request.Method == "GET")
                    {
                        if (request.Route == "/")
                        {
                            string filePath = rootPath + "index.html";

                            try
                            {
                                response.Body = System.IO.File.ReadAllText(filePath);

                            }
                            catch (FileNotFoundException)
                            {

                                response.Body = "Index file Not Found";
                                response.StatusLine = "404 Not Found";
                            }
                        }
                        else if (request.Route == "/about")
                        {
                            string filePath = rootPath + "about.html";

                            try
                            {
                                response.Body = System.IO.File.ReadAllText(filePath);
                            }
                            catch (FileNotFoundException)
                            {

                                response.Body = "file Not Found";
                                response.StatusLine = "404 Not Found";
                            }

                        }
                        else if (request.Route == "/contact")
                        {
                            string filePath = rootPath + "contact.html";

                            try
                            {
                                response.Body = System.IO.File.ReadAllText(filePath);
                            }
                            catch (FileNotFoundException)
                            {

                                response.Body = "file Not Found";
                                response.StatusLine = "404 Not Found";
                            }
                        }
                        else if (request.Route == "/style.css")
                        {
                            string filePath = rootPath + "style.css";

                            try
                            {
                                response.Body = System.IO.File.ReadAllText(filePath);
                                response.ContentType = "text/css; charset=utf-8";
                            }
                            catch (FileNotFoundException)
                            {

                                response.Body = "Index file Not Found";
                                response.StatusLine = "404 Not Found";
                            }

                        }
                        else if (request.Route == "/script.js")
                        {
                            string filePath = rootPath + "script.js";

                            try
                            {
                                response.Body = System.IO.File.ReadAllText(filePath);
                                response.ContentType = "application/javascript; charset=utf-8";
                            }
                            catch (FileNotFoundException)
                            {

                                response.Body = "Index file Not Found";
                                response.StatusLine = "404 Not Found";
                            }

                        }
                        else if (request.Route == "/img.png")
                        {
                            string filePath = rootPath + "img.png";

                            try
                            {
                                response.BodyBytes = System.IO.File.ReadAllBytes(filePath);
                                response.ContentType = "image/png";

                            }
                            catch (FileNotFoundException)
                            {

                                response.Body = "Image Not Found";
                                response.StatusLine = "404 Not Found";
                            }

                        }
                        else
                        {
                            response.StatusLine = "404 Not Found";
                        }


                    }

                    else if (request.Method == "POST")
                    {
                        if (request.Route == "/contact")
                        {
                            request.Body = dataRead.Split("\r\n\r\n")[1];
                            string[] keyValue = request.Body.Split('&');
                            string[] key = new string[keyValue.Length];
                            string[] value = new string[keyValue.Length];

                            for (int i = 0; i < keyValue.Length; i++)
                            {
                                string[] pair = keyValue[i].Split('=');

                                key[i] = pair[0];
                                value[i] = pair[1];
                            }

                            response.Body = $"<h1>Form Submitted</h1>\r\n\r\n<p>{key[0]}: {value[0]}</p>\r\n\r\n<p>{key[1]}: {value[1]}</p>";
                            response.ContentType = "text/html; charset=utf-8";
                            
                        }
                    }

                    if (response.BodyBytes != null)
                    {
                        numBytes = response.BodyBytes.Length;
                    }
                    else
                    {
                        numBytes = Encoding.UTF8.GetByteCount(response.Body);
                    }

                    string responseMsg = $"""
                                  HTTP/1.1 {response.StatusLine}
                                  Content-Type: {response.ContentType}
                                  Content-Length: {numBytes}

                                  
                                  """;

                    byte[] responseBytes = Encoding.UTF8.GetBytes(responseMsg);

                    stream.Write(responseBytes, 0, responseBytes.Length);

                    if (response.BodyBytes != null)
                    {
                        stream.Write(response.BodyBytes, 0, response.BodyBytes.Length);
                    }
                    else
                    {
                        byte[] textBytes = Encoding.UTF8.GetBytes(response.Body);
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
