namespace HttpServer
{
    public class HttpRequest
    {
        public string Method { get; set; }

        public string Route { get; set; }

        public string Version { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string Body { get; set; }
    }
}