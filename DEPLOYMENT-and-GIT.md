# Putting your project online — Git & Deployment

A short reference for Session 19. Your app has **two halves** that are
deployed to **two different kinds of host**.

| Part | What it is | Where it goes |
|------|-----------|---------------|
| Front end | static HTML / CSS / JS (`frontend/`) | a **static host** — GitHub Pages, Netlify, Vercel |
| Back end | your C# ASP.NET Core Web API (`backend/`) | a **host that runs .NET** — Azure App Service, Render |
| Database | the SQLite file / a hosted DB | set via configuration on the API host |

---

## 1. Put the code on GitHub (using VS Code — no terminal needed)

1. Open the project folder in VS Code.
2. Click the **Source Control** icon on the left (the branch symbol).
3. Click **Initialize Repository**.
4. Type a commit message (e.g. "First version"), then click **✓ Commit**.
5. Click **Publish to GitHub** and choose a public or private repo.

That's it — your code is on GitHub. Commit again whenever you make changes.

_(Terminal equivalent, for reference:)_

```
git init
git add .
git commit -m "First version"
git push
```

## 2. Deploy the front end (static)

- **GitHub Pages:** in your GitHub repo → Settings → Pages → deploy from the
  main branch. Your site gets a public URL.
- **Netlify / Vercel:** sign in with GitHub, pick the repo, deploy. Drag-and-drop
  also works for a quick test.

## 3. Deploy the back end (the C# API)

- Publish the API to a .NET host (e.g. **Azure App Service** or **Render**).
- Set the database **connection string** through the host's configuration
  (environment variables) — never hard-code secrets.
- Make sure **CORS** allows your front-end URL (you set this up in Session 15).

## 4. Connect the two

Open `app.js` and change the one line at the top:

```js
const API_BASE = "https://your-live-api.com/api/products";
```

Commit, and your deployed front end now talks to your deployed API. 🎉

---

### Watch out for
- **CORS errors** — the API must allow your front-end's origin.
- **http vs https** — a page served over https can't call an http API.
- **Never commit secrets** — keep connection strings / keys in host config.
