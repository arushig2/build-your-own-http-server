# 02 - Reading Raw HTTP Requests

## Objective

The goal of this milestone was to read the raw data sent by a client after establishing a TCP connection.

Instead of relying on a web framework to handle HTTP, this milestone focused on understanding that an HTTP request is simply a sequence of bytes transmitted over a TCP connection.

---

## From `TcpClient` to `NetworkStream`

After a client connects, `AcceptTcpClient()` returns a `TcpClient` object representing that specific connection.

However, the data is not read directly from the `TcpClient`. Instead, it provides access to a `NetworkStream`, which allows us to read the bytes being sent by the client.

```csharp
using NetworkStream stream = client.GetStream();
```

A `NetworkStream` represents a continuous stream of bytes flowing between the client and the server.

This follows a common design pattern in .NET:

- `FileStream` → Reads bytes from a file.
- `MemoryStream` → Reads bytes from memory.
- `NetworkStream` → Reads bytes from a network connection.

---

## Reading Data from the Stream

To receive data from the client, we first create a buffer.

```csharp
byte[] buffer = new byte[1024];
```

The buffer acts as temporary storage for the incoming bytes.

We then read data from the stream.

```csharp
int bytesRead = stream.Read(buffer, 0, buffer.Length);
```

### Understanding `Read()`

`Read()` does **not** return the data itself.

Instead, it:

1. Reads bytes from the network.
2. Copies those bytes into the provided buffer.
3. Returns the number of bytes that were actually read.

For example:

- Buffer size = **1024 bytes**
- Data received = **699 bytes**

The first 699 bytes contain valid data, while the remaining bytes in the buffer are unused.

---

## Converting Bytes into Text

HTTP requests are transmitted as bytes.

To display them in a readable format, the bytes need to be decoded into a string.

```csharp
string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
```

Notice that only the bytes that were actually read are converted.

Using `bytesRead` ensures that unused bytes in the buffer are ignored.

---

## The First HTTP Request

After running the server and opening:

```text
http://localhost:8080
```

the browser sent a request similar to:

```http
GET / HTTP/1.1
Host: localhost:8080
Connection: keep-alive
User-Agent: Mozilla/5.0 ...
Accept: text/html,...
```

Seeing this request demonstrated that HTTP is not a special object or protocol hidden inside the browser—it's simply text transmitted over a TCP connection.

---

## Understanding the Request

The first line is called the **Request Line**.

```http
GET / HTTP/1.1
```

It consists of three parts:

| Part | Description |
|------|-------------|
| `GET` | HTTP Method |
| `/` | Requested Path |
| `HTTP/1.1` | HTTP Version |

Everything below the request line consists of **HTTP Headers**.

Headers provide additional information about the request, such as:

- Host
- Browser information (`User-Agent`)
- Accepted response formats (`Accept`)
- Connection preferences (`Connection`)

---

## Key Takeaways

- `TcpClient` represents a single client connection.
- `NetworkStream` provides access to the bytes exchanged over that connection.
- `Read()` fills a buffer and returns the number of bytes that were read.
- The size of the buffer and the amount of data received are not necessarily the same.
- `Encoding.UTF8.GetString(buffer, 0, bytesRead)` converts only the valid bytes into a string.
- HTTP requests are plain text transmitted over TCP.

---

## Current Progress

- ✅ Accepted a TCP client connection.
- ✅ Read bytes from the network.
- ✅ Converted bytes into a string.
- ✅ Displayed a raw HTTP request sent by the browser.

---

## Next Milestone

Parse the raw HTTP request into a structured object by extracting:

- HTTP Method
- Request Path
- HTTP Version
- Request Headers