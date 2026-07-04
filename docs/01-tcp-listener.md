# 01 - TCP Listener

## Objective

The goal of this milestone was to build a basic TCP server that listens for incoming client connections. At this stage, the server does **not** understand HTTP—it only establishes a TCP connection with a client.

---

## What is a TCP Server?

A TCP server is an application that listens on a specific network port and accepts incoming TCP connections from clients.

Unlike a normal console application that performs a task and exits, a server keeps running so that clients can connect whenever they need to communicate.

In this project, the server listens on **port 8080**.

---

## Understanding the Components

### `TcpListener`

`TcpListener` is responsible for listening for incoming TCP connection requests.

You can think of it as opening a gate. Once the gate is open, clients can arrive and request a connection. The listener itself does not communicate with clients; its primary responsibility is to wait for and accept new connections.

---

### `IPEndPoint`

An `IPEndPoint` specifies **where** the server should listen.

It consists of:

- An IP address
- A Port number

Example:

```csharp
var ipEndPoint = new IPEndPoint(IPAddress.Any, 8080);
```

`IPAddress.Any` tells the server to listen on all available network interfaces, while `8080` specifies the port on which incoming TCP connections will be accepted.

---

### `listener.Start()`

Calling `listener.Start()` does not continuously run the server.

Instead, it registers the listener with the operating system and tells it:

> "I want to receive TCP connections on port 8080."

From that point onward, the operating system monitors that port and notifies the application whenever a client attempts to connect.

---

### `AcceptTcpClient()`

`AcceptTcpClient()` waits for an incoming client connection.

This method is **blocking**, meaning the current thread pauses until a client connects.

It does **not** return `null` if no client is available. Instead, it waits efficiently without repeatedly checking for new connections.

Once a client connects, the method returns a `TcpClient` object representing that specific connection.

---

## What Happens Behind the Scenes?

The sequence of events looks like this:

```text
Application Starts
        │
        ▼
Create TcpListener
        │
        ▼
listener.Start()
        │
        ▼
Operating System starts listening on port 8080
        │
        ▼
AcceptTcpClient()
        │
        │   (Waiting...)
        │
        ▼
Browser connects
        │
        ▼
Operating System notifies the application
        │
        ▼
AcceptTcpClient() returns a TcpClient
```

---

## Role of the Operating System

Before calling `listener.Start()`, no application is listening on port **8080**.

When `listener.Start()` is executed, the application requests the operating system to associate port **8080** with the current process.

When a client (such as a browser) tries to connect, the operating system receives the connection request first and forwards it to the application that owns that port.

The application does not continuously check for clients; the operating system wakes it only when a connection is available.

---

## Questions I Had

### Why doesn't `AcceptTcpClient()` return `null` if no client is connected?

If it returned `null`, the application would have to repeatedly check whether a client had connected, resulting in unnecessary CPU usage.

Instead, `AcceptTcpClient()` blocks the current thread until a connection is available. This is a much more efficient approach and is commonly used by server applications.

---

### What happens after `listener.Start()`?

`listener.Start()` tells the operating system that the application wants to listen for TCP connections on a specific port.

The operating system then takes responsibility for monitoring that port and notifying the application whenever a client connects.

---

### Why did the application stop immediately in the first version?

Initially, the server started listening and then immediately reached the end of the `Start()` method.

Once the method returned, the application had nothing left to execute, so it exited and the listener stopped.

Adding `AcceptTcpClient()` kept the application alive by waiting for an incoming connection.

---

## Key Takeaways

- A TCP server listens for incoming TCP connections.
- `TcpListener` waits for and accepts client connections.
- `IPEndPoint` defines the IP address and port on which the server listens.
- `listener.Start()` registers the listener with the operating system.
- `AcceptTcpClient()` is a blocking call that waits efficiently for a client connection.
- The operating system manages incoming network connections and wakes the application when a client connects.

---

## Current Progress

- ✅ Created a TCP server
- ✅ Started listening on port **8080**
- ✅ Accepted the first client connection

### Next Milestone

Read the raw bytes sent by the client and inspect the HTTP request.