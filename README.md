# Build Your Own HTTP Server

A lightweight HTTP/1.1 server built from scratch in **C#**, implementing the protocol directly over **TCP sockets** without relying on ASP.NET Core or any existing web server framework.

## About

This project is part of my **Build Your Own** series, where I recreate core software systems to better understand how they work internally.

The server accepts raw TCP connections, parses HTTP requests, performs request routing, serves static resources, handles form submissions, and constructs HTTP responses manually. The goal is to understand the fundamentals of the HTTP protocol rather than depend on high-level web frameworks.

## Features

* ✅ TCP server built with `TcpListener`
* ✅ Accept and process client connections
* ✅ Parse raw HTTP/1.1 requests
* ✅ Generate compliant HTTP responses
* ✅ Support GET and POST requests
* ✅ Basic request routing
* ✅ Serve static HTML, CSS, JavaScript, and image files
* ✅ Handle HTTP response headers (`Content-Type`, `Content-Length`)
* ✅ Separate `HttpRequest` and `HttpResponse` models
* ✅ Modular server architecture with helper methods for request handling and response generation

## Technologies Used

* C#
* .NET 10
* TCP Sockets (`TcpListener`, `TcpClient`, `NetworkStream`)
* HTTP/1.1

## Project Structure

```text
build-your-own-http-server/
│
├── docs/                     # Milestone documentation
├── src/
│   └── HttpServer/
│       ├── HttpRequest.cs
│       ├── HttpResponse.cs
│       ├── Program.cs
│       ├── Server.cs
│       └── wwwroot/
│           ├── index.html
│           ├── about.html
│           ├── contact.html
│           ├── style.css
│           ├── script.js
│           └── img.png
│
└── README.md
```

## Request Lifecycle

1. Start a TCP listener on port **8080**.
2. Accept an incoming client connection.
3. Read the raw HTTP request from the network stream.
4. Parse the request line and request body.
5. Route the request based on the HTTP method and URL.
6. Serve static content or process the request.
7. Build an HTTP response with the appropriate status, headers, and body.
8. Send the response back to the client.

## Learning Outcomes

Building this project helped me understand:

* TCP socket programming
* The HTTP/1.1 request-response lifecycle
* HTTP methods (GET and POST)
* Request routing
* Static file serving
* Binary data transmission
* HTTP response headers and MIME types
* Parsing raw HTTP messages
* Writing clean, modular C# code

## Getting Started

### Prerequisites

* .NET 10 SDK

### Run the Project

```bash
git clone <repository-url>
cd build-your-own-http-server
dotnet run --project src/HttpServer/HttpServer.csproj
```

Once the server starts, open your browser and visit:

```text
http://localhost:8080
```

## Future Improvements

* Parse request headers into structured objects
* Support query parameters
* Dynamic route registration
* Persistent (Keep-Alive) connections
* Asynchronous request handling
* Middleware pipeline
* Unit and integration tests

## License

This project is part of my **Build Your Own** learning series and is intended for educational purposes.
