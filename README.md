# Session 19 — Full-stack app (real API + real database)

This is the whole thing put together. There is **no `products.json`** and **no
demo data in the browser**. The web page is only a *client*: every product it
shows, adds, or deletes goes through your **C# Web API**, which stores the data
in a real **SQLite database** — exactly the API you built in Week 3 (Session 14).

```
Session19_FullStack/
├── backend/     ← C# ASP.NET Core Web API + EF Core + SQLite   (the server)
└── frontend/    ← HTML / CSS / JS                              (the client)
```

The browser page (frontend) calls the API (backend) with `fetch`. The API reads
and writes the `shop.db` SQLite file. That's a full stack: **UI → API → database.**

---

## How to run it (two parts, two windows)

You start the **backend first**, then open the **frontend**.

### 1. Start the backend (the API)

The very first time on a machine with internet, download the packages:

```
cd backend
dotnet restore
```

Then run it:

```
dotnet run
```

- On first run it creates **`shop.db`** and seeds six sample products.
- Leave this window running. It prints its address — it should be
  **http://localhost:5000** (that's the address the front end expects).
- Quick check: open <http://localhost:5000/api/products> in a browser — you
  should see the products as JSON.

> **Note (important for our classroom machines):** `dotnet restore` needs
> internet the first time, because it downloads Entity Framework Core. Do this
> once while online; after that the app runs offline.

### 2. Open the frontend (the web page)

The page uses `fetch`, so open it through a tiny local web server (not by
double-clicking the file). In a **second** terminal:

```
cd frontend
```

Then start any static server, for example:

- **VS Code:** right-click `index.html` → **Open with Live Server**
  (install the "Live Server" extension once).
- **Or Python:** `python -m http.server 8000`, then open
  <http://localhost:8000>.

You'll see the six products load from the database. Add one with the form, or
delete one — then **refresh the page**. The change is still there, because it's
saved in the database on the server, not in the browser.

---

## Why CORS is switched on

The page runs at `http://localhost:8000` and the API at `http://localhost:5000`
— two different addresses, so the browser treats the call as *cross-origin* and
blocks it unless the API allows it. `Program.cs` adds a CORS policy
(`AllowAnyOrigin`) so the front end can call the API during development. In a
real deployment you'd restrict that to your site's address.

## What connects to what

| Action on the page | Front end (`app.js`) | Back end endpoint | Database |
|--------------------|----------------------|-------------------|----------|
| Page loads         | `GET` fetch          | `GET /api/products`      | `SELECT` all rows |
| Add product        | `POST` fetch         | `POST /api/products`     | `INSERT` a row |
| Delete product     | `DELETE` fetch       | `DELETE /api/products/{id}` | `DELETE` a row |

If the page shows a red "Can't reach the API" message, the backend isn't
running (or isn't on port 5000). Start it and refresh.

See **DEPLOYMENT-and-GIT.md** for putting both halves online.
