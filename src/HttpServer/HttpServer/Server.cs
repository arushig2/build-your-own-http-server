using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpServer
{
    public class Server
    {
        private const string RootPath = "../../../wwwroot/";
        private readonly Dictionary<string, string> fileToContentTypeMapping = new()
        {
            { "html", "text/html" },
            { "css", "text/css" },
            { "js", "application/javascript" }
        };
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

                    using TcpClient client = listener.AcceptTcpClient();
                    using NetworkStream stream = client.GetStream();

                    HttpRequest request = ReadRequest(stream);
                    HttpResponse response = new HttpResponse();
                
                    if (request.Method == "GET")
                    {
                        HandleGetRequest(request, response);
                    }

                    else if (request.Method == "POST")
                    {
                        HandlePostRequest(request, response);
                    }

                    SendResponse(response, stream);
                }               

            }
            finally
            {
                listener.Stop();
                Console.WriteLine($"Stopped listening on port {Port}...");
            }
        }

        private HttpRequest ReadRequest(NetworkStream stream)
        {
            HttpRequest request = new HttpRequest();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer);

            string dataRead = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Data received : {dataRead}");

            string requestLine = dataRead.Split('\n')[0].Trim();
            string[] requestParts = requestLine.Split(' ', 3);

            request.Method = requestParts[0];
            request.Route = requestParts[1];
            request.Version = requestParts[2];

            string[] requestSections = dataRead.Split("\r\n\r\n", 2);

            request.Body = requestSections.Length > 1
                ? requestSections[1]
                : string.Empty;

            return request;
        }

        private void HandleGetRequest(HttpRequest request, HttpResponse response)
        {
            if (request.Route == "/")
            {
                ServeTextFile("index.html", response);
            }
            else if (request.Route == "/about")
            {
                ServeTextFile("about.html", response);

            }
            else if (request.Route == "/contact")
            {
                ServeTextFile("contact.html", response);
            }
            else if (request.Route == "/style.css")
            {
                ServeTextFile("style.css", response);

            }
            else if (request.Route == "/script.js")
            {
                ServeTextFile("script.js", response);

            }
            else if (request.Route == "/img.png")
            {
                ServeImageFile("img.png", response);
            }
            else
            {
                response.StatusLine = "404 Not Found";
            }
        }

        private void HandlePostRequest(HttpRequest request, HttpResponse response)
        {
            if (request.Route == "/contact")
            {
                
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

        private void ServeTextFile(string fileName, HttpResponse response)
        {
            string fileType = Path.GetExtension(fileName).TrimStart('.');
            string filePath = Path.Combine(RootPath, fileName);

            try
            {
                response.Body = File.ReadAllText(filePath);
                response.ContentType = $"{fileToContentTypeMapping[fileType]}; charset=utf-8";

            }
            catch (FileNotFoundException)
            {
                response.Body = "file Not Found";
                response.StatusLine = "404 Not Found";
            }
        }

        private void ServeImageFile(string fileName, HttpResponse response)
        {
            string filePath = Path.Combine(RootPath, fileName);

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
   
        private void SendResponse(HttpResponse response, NetworkStream stream)
        {
            int numBytes = 0;
            if (response.BodyBytes != null)
            {
                numBytes = response.BodyBytes.Length;
            }
            else
            {
                numBytes = Encoding.UTF8.GetByteCount(response.Body);
            }

            string responseMessage = $"""
                                  HTTP/1.1 {response.StatusLine}
                                  Content-Type: {response.ContentType}
                                  Content-Length: {numBytes}

                                  
                                  """;

            byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);

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
        }
    }
}
