# 03 - HTTP Response Handling

## Objective

In the previous milestone, the server was able to receive and parse an HTTP request. However, it did not send any response back to the client. The goal of this milestone was to understand the structure of an HTTP response and make the server return valid responses that a web browser can interpret.

---

## What is an HTTP Response?

After receiving an HTTP request, the server sends an HTTP response back to the client. A response consists of four parts:

1. Status Line
2. Response Headers
3. Blank Line
4. Response Body

Example:

```http
HTTP/1.1 200 OK
Content-Type: text/html; charset=utf-8
Content-Length: 11

Hello World
```

The blank line separates the response headers from the response body.

---

## Response Components

### Status Line

The status line contains:

- HTTP Version
- Status Code
- Reason Phrase

Example:

```http
HTTP/1.1 200 OK
```

For this milestone, every response returns `200 OK`. Proper status codes such as `404 Not Found` will be implemented in a later milestone.

### Response Headers

Headers provide metadata about the response.

The server currently sends the following headers:

#### Content-Type

```http
Content-Type: text/html; charset=utf-8
```

This tells the browser that the response body contains HTML encoded using UTF-8.

#### Content-Length

```http
Content-Length: 11
```

This specifies the size of the response body in **bytes**, not characters.

Instead of hardcoding this value, it is calculated dynamically based on the response body.

### Blank Line

A blank line marks the end of the response headers and the beginning of the response body.

Without this separator, the client cannot determine where the headers end and the response body begins.

### Response Body

The response body contains the actual content displayed to the client.

Examples used in this milestone include:

- Home Page
- About Page
- Contact us at example@email.com
- Invalid Route

---

## Why Content-Length Matters

HTTP transmits data as bytes.

The `Content-Length` header tells the client exactly how many bytes belong to the response body.

If the value is incorrect, the client may continue waiting for additional data or treat the response as incomplete.

The byte count is calculated using UTF-8 encoding because a character is not always represented by a single byte.

---

## Basic Routing

The first line of an HTTP request is called the **request line**.

Example:

```http
GET /about HTTP/1.1
```

The request line contains three parts:

- HTTP Method
- Request Path
- HTTP Version

The server extracts the request path and uses it to determine the response body.

Current routes:

| Route | Response |
| ------ | -------- |
| `/` | Home Page |
| `/about` | About Page |
| `/contact` | Contact us at example@email.com |
| Any other route | Invalid Route |

Only the response body changes for different routes. The overall HTTP response structure remains the same.

---

## Key Learnings

- Understood the structure of an HTTP response.
- Learned the purpose of the status line, headers, blank line, and response body.
- Calculated `Content-Length` dynamically instead of hardcoding it.
- Learned the difference between character count and byte count.
- Understood the purpose of the `Content-Type` header and MIME types.
- Parsed the request line to extract the requested route.
- Implemented basic routing based on the requested path.
- Reused the same HTTP response format while changing only the response body.

---

## Next Milestone

The server currently returns `200 OK` for every request, including invalid routes.

In the next milestone, the server will return appropriate HTTP status codes such as:

- `200 OK`
- `404 Not Found`
- `400 Bad Request`

This will make the server more compliant with the HTTP specification.