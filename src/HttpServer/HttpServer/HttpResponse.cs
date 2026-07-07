namespace HttpServer
{
    public class HttpResponse
    {
        public string Body { get; set; }

        public byte[]? BodyBytes { get; set; }

        public string StatusLine { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string ContentType { get; set; }

    }
}