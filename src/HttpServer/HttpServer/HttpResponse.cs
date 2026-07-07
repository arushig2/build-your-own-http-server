namespace HttpServer
{
    public class HttpResponse
    {
        public string Body { get; set; }

        public byte[]? BodyBytes { get; set; }

        public string StatusLine { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string ContentType { get; set; }

        public HttpResponse()
        {
            StatusLine = "200 OK";
            ContentType = "text/html; charset=utf-8";
            Body = "Invalid Route";
            BodyBytes = null;
        }
        

    }
}