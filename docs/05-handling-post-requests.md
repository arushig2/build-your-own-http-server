# 05 - Handling POST Requests

## Overview

So far, the server was capable of handling **GET** requests to serve HTML pages and static assets. While this allowed clients to retrieve resources, it did not support sending data back to the server.

In this milestone, support for **POST** requests was added, allowing the server to receive and process form data submitted by a client.

---

## What is a POST Request?

Unlike a GET request, which is primarily used to retrieve resources, a POST request sends data to the server inside the **request body**.

For example, submitting the following HTML form:

```html
<form action="/contact" method="POST">
    <input name="name">
    <textarea name="message"></textarea>
    <button type="submit">Send</button>
</form>
```

generates a request similar to:

```http
POST /contact HTTP/1.1
Host: localhost:8080
Content-Type: application/x-www-form-urlencoded
Content-Length: 22

name=Arushi&message=Hi
```

The important difference is the presence of a **request body** after the blank line separating the headers from the body.

---

## Changes Made

### 1. Parse the HTTP Method

Previously, only the requested route was extracted from the request line. The server now also extracts the HTTP method.

```text
GET /about HTTP/1.1
│    │
│    └── Route
└──────── Method
```

This allows the server to distinguish between requests such as:

- `GET /contact`
- `POST /contact`

even though they target the same route.

---

### 2. Route Requests by Method

Routing is now performed based on both the HTTP method and the requested route.

Example:

- `GET /contact` → Returns the contact form.
- `POST /contact` → Processes the submitted form data.

---

### 3. Read the Request Body

The entire HTTP request is already available after reading from the network stream.

For POST requests, the server separates the headers from the body using the HTTP header-body delimiter:

```text
\r\n\r\n
```

Everything after this delimiter is treated as the request body.

---

### 4. Parse Form Data

HTML forms using the default content type (`application/x-www-form-urlencoded`) send data as key-value pairs.

Example:

```text
name=Arushi&message=Hi
```

The server parses this by:

1. Splitting the body using `&`
2. Splitting each pair using `=`

Result:

| Key | Value |
|------|-------|
| name | Arushi |
| message | Hi |

---

### 5. Generate a Dynamic Response

Instead of returning a static page, the server generates an HTML response displaying the submitted form values.

Example response:

```html
<h1>Form Submitted</h1>

<p>Name: Arushi</p>

<p>Message: Hi</p>
```

This demonstrates that the server successfully received and processed client-submitted data.

---

## Concepts Learned

- Difference between GET and POST requests
- HTTP request body
- Header-body separation using `\r\n\r\n`
- Parsing form data
- Handling `application/x-www-form-urlencoded`
- Routing based on both HTTP method and route
- Generating dynamic HTML responses

---

## Current Server Capabilities

The server can now:

- Accept TCP connections
- Parse HTTP requests
- Handle GET requests
- Handle POST requests
- Serve HTML pages
- Serve CSS, JavaScript, and images
- Return appropriate status codes
- Read request bodies
- Process HTML form submissions
- Generate dynamic responses

---

## Next Steps

The server is now functional but has accumulated a significant amount of routing and response-handling logic inside the `Start()` method.

The next milestone will focus on **refactoring** the code by introducing dedicated request and response objects, reducing duplication, and improving the overall structure without changing the server's functionality.