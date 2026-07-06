# Milestone 4 – Serving Static Content

## Objective

Enhance the HTTP server to serve static files instead of plain text responses. The server now delivers HTML pages as well as supporting assets such as CSS, JavaScript, and images, making it capable of serving a basic website.

---

## Features Implemented

### HTML File Serving

- Added support for serving `index.html` for the root route (`/`).
- Added support for serving `about.html` for the `/about` route.
- Used `File.ReadAllText()` to read and return HTML files.

### Static Asset Support

The server can now serve:

- CSS files
- JavaScript files
- PNG images

### MIME Types

Implemented appropriate `Content-Type` headers for different resource types.

| File Type | Content-Type |
|-----------|--------------|
| HTML | `text/html; charset=utf-8` |
| CSS | `text/css; charset=utf-8` |
| JavaScript | `application/javascript; charset=utf-8` |
| PNG | `image/png` |

### Binary File Handling

Introduced binary response handling for image files using `File.ReadAllBytes()`.

Unlike HTML, CSS, and JavaScript, image files cannot be treated as text and must be transmitted as raw bytes.

### Multiple Browser Requests

Modified the server to remain active and handle multiple incoming requests from the browser, allowing HTML pages and their associated assets to be loaded successfully.

---

## Concepts Learned

- Serving static files
- MIME types (`Content-Type`)
- Difference between text and binary files
- `File.ReadAllText()` vs `File.ReadAllBytes()`
- Constructing HTTP responses using headers and body separately
- Why browsers generate multiple HTTP requests for a single webpage

---

## Current Limitations

- Routes are manually mapped using `if-else` statements.
- Static file serving is not generic.
- Requests are processed sequentially.
- Only a limited set of MIME types is supported.

These limitations will be addressed in later milestones through refactoring and improved routing.

---

## Outcome

The server is now capable of serving a simple static website, including HTML pages, stylesheets, JavaScript files, and images, providing a better understanding of how web servers deliver static content over HTTP.