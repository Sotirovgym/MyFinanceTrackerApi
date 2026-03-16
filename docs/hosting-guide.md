# Hosting guide: .NET API + React SPA (first deployment)

This guide walks you through hosting MyFinanceTracker for the first time: **API** (ASP.NET Core), **SPA** (React), and **PostgreSQL** database.

---

## What you need to host

| Piece | What it is | Where it runs |
|-------|------------|---------------|
| **PostgreSQL** | Database (users, accounts, categories, transactions) | Managed DB service |
| **.NET API** | Backend (auth, CRUD, business logic) | App-hosting service |
| **React SPA** | Frontend (build output = static files) | Static hosting or same server |

The React app will call your API over HTTPS. The API must allow requests from your SPA’s origin (CORS).

---

## Recommended path for your first time

Use **one simple platform** for API + DB, and **one** for the SPA, so you have fewer accounts and concepts.

**Option A – Easiest (all-in-one)**  
- **Railway** (railway.app): Host API + PostgreSQL in one project. Free tier has limits but is enough for an MVP.  
- **Vercel** or **Netlify**: Host the React app (static site). Free tier.

**Option B – Very common**  
- **Render** (render.com): API (Web Service) + PostgreSQL. Free tier.  
- **Vercel** or **Netlify**: React SPA.

**Option C – Microsoft stack**  
- **Azure**: App Service (API) + Azure Database for PostgreSQL (or Flexible Server) + Static Web Apps (React). More setup, good if you want to learn Azure.

Below we use **Option A (Railway + Vercel)** as the main example. Steps for Render are similar.

---

## Before you start

1. **Code ready**
   - API runs locally and migrations are applied.
   - React app runs locally and can login + call API (e.g. `http://localhost:5000` or your API URL).

2. **Accounts**
   - [Railway](https://railway.app) (or [Render](https://render.com)) for API + DB.
   - [Vercel](https://vercel.com) (or [Netlify](https://netlify.com)) for React.

3. **Secrets you’ll need**
   - A **strong JWT secret** (e.g. 32+ random characters). Never commit this.
   - PostgreSQL connection string (Railway/Render will give you this).

---

## Part 1: Prepare the API for production

### 1.1 CORS (so the SPA can call the API)

CORS is already configured in the API: it reads allowed origins from **Cors:AllowedOrigins** in config (default `http://localhost:3000` for local React dev).

- **Local:** `appsettings.json` has `"Cors": { "AllowedOrigins": [ "http://localhost:3000" ] }`.
- **Production:** Set the real SPA URL via environment variables, e.g. `Cors__AllowedOrigins__0` = `https://my-finance-tracker.vercel.app` (see Part 2.3).

### 1.2 Use environment variables for secrets

In production you will **not** use `appsettings.json` for secrets. Use:

- **ConnectionStrings:DefaultConnection**
- **Jwt:Secret**
- **Cors:AllowedOrigins** (or a single URL)

On Railway/Render you’ll set these as **environment variables**. ASP.NET Core maps them automatically (e.g. `ConnectionStrings__DefaultConnection`, `Jwt__Secret`).

### 1.3 Optional: Swagger only in non-production

So Swagger isn’t exposed in production:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

(You may already have this.)

### 1.4 Build and run locally once more

```bash
cd MyFinanceTracker.Api
dotnet publish -c Release -o ./publish
```

Check that the published folder runs (e.g. set env vars and run the DLL). Fix any path or config issues before deploying.

---

## Part 2: Host PostgreSQL and the API (Railway)

### 2.1 Create a project and add PostgreSQL

1. Go to [railway.app](https://railway.app), sign in (e.g. GitHub).
2. **New Project** → **Deploy from GitHub repo** (connect your repo) or **Empty project** and we’ll add services.
3. In the project: **+ New** → **Database** → **PostgreSQL**.  
   Railway creates a DB and gives you a **connection URL** (e.g. `postgresql://user:pass@host:port/railway`).
4. Open the PostgreSQL service → **Variables** (or **Connect**) and copy **DATABASE_URL** or the connection string.

### 2.2 Add the .NET API service

1. In the same project: **+ New** → **GitHub Repo** (or **Empty Service** if you deploy manually).
2. If using GitHub: select your repo. Set **Root Directory** to the API project folder if the repo is a monorepo (e.g. leave empty if the repo root is the solution and the API is in `MyFinanceTracker.Api`).
3. **Settings** for the API service:
   - **Build command:**  
     `dotnet publish MyFinanceTracker.Api/MyFinanceTracker.Api.csproj -c Release -o out`
   - **Start command:**  
     `dotnet out/MyFinanceTracker.Api.dll`
   - **Watch paths:** e.g. `MyFinanceTracker.Api/**` so only API changes trigger a deploy.

### 2.3 Set environment variables (Railway)

In the **API service** → **Variables**, add:

| Variable | Value | Notes |
|----------|--------|--------|
| `ConnectionStrings__DefaultConnection` | Your PostgreSQL connection string | From the DB service (copy from Railway’s PostgreSQL variables). |
| `Jwt__Secret` | A long random string (32+ chars) | Generate once, keep secret. |
| `Jwt__Issuer` | `MyFinanceTracker` | Or your app name. |
| `Jwt__Audience` | `MyFinanceTracker` | Same. |
| `Jwt__ExpirationMinutes` | `60` | Or 1440 for 24h. |
| `ASPNETCORE_ENVIRONMENT` | `Production` | So .NET runs in Production mode. |
| `Cors__AllowedOrigins__0` | `https://your-react-app.vercel.app` | Your real SPA URL (after you deploy the SPA). For testing you can use `*` temporarily (less secure). |

Railway will assign a public URL like `https://your-api.up.railway.app`. Note it for the React app.

### 2.4 Run migrations on the hosted database

Your API runs migrations at startup only if you added that. If not, run them once manually:

**Option 1 – From your machine (recommended)**  
Set `ConnectionStrings__DefaultConnection` to the **same** Railway PostgreSQL URL and run:

```bash
cd MyFinanceTracker.Api
dotnet ef database update --project ../MyFinanceTracker.Infrastructure
```

**Option 2 – Run migrations inside Railway**  
Add a one-off start script or a deploy step that runs `dotnet ef database update` against the same connection string. (Details depend on Railway’s build/deploy steps.)

### 2.5 Check the API

- Open `https://your-api.up.railway.app/swagger` (if Swagger is enabled in production) or call `https://your-api.up.railway.app/api/Auth/login` with a POST and JSON body to see a 400/401 (so the app is alive).

---

## Part 3: Host the React SPA (Vercel)

### 3.1 Build the React app against the production API

In your React app, the API base URL should be configurable (env variable), e.g.:

- **Development:** `http://localhost:5000` (or your local API URL).
- **Production:** `https://your-api.up.railway.app`.

Example with Vite (`.env.production`):

```env
VITE_API_URL=https://your-api.up.railway.app
```

Use `import.meta.env.VITE_API_URL` (or your framework’s env) when calling the API. Build:

```bash
npm run build
```

### 3.2 Deploy to Vercel

1. Go to [vercel.com](https://vercel.com), sign in (e.g. GitHub).
2. **Add New** → **Project** → import the repo that contains the React app.
3. **Root Directory:** if the React app is in a subfolder (e.g. `frontend`), set it.
4. **Build Command:** `npm run build` (or `yarn build`).
5. **Output Directory:** `dist` (Vite) or `build` (Create React App).
6. **Environment Variables:** add `VITE_API_URL` = `https://your-api.up.railway.app`.
7. Deploy. Vercel gives you a URL like `https://my-finance-tracker.vercel.app`.

### 3.3 Point the API CORS at the real SPA URL

Back in Railway (API service) → **Variables**, set:

- `Cors__AllowedOrigins__0` = `https://my-finance-tracker.vercel.app` (your real Vercel URL).

Redeploy the API if needed so the new CORS setting is applied.

### 3.4 Test the full flow

1. Open the Vercel URL.
2. Register a new user.
3. Log in, create an account/category, add a transaction.  
If something fails, check the browser console (CORS, 404, 401) and the API logs on Railway.

---

## Part 4: If you use Render instead of Railway

- **PostgreSQL:** Create a **PostgreSQL** instance in Render; copy the **Internal** or **External** connection string.
- **API:** Create a **Web Service**, connect the repo, set:
  - **Build:** `dotnet publish MyFinanceTracker.Api/MyFinanceTracker.Api.csproj -c Release -o out`
  - **Start:** `dotnet out/MyFinanceTracker.Api.dll`
- **Environment:** Same variables as in 2.3; set them in the Render **Environment** tab.
- Run migrations the same way (from your machine with the Render DB URL, or via a deploy script).
- CORS: set your Vercel/Netlify SPA URL in the API’s env.

---

## Part 5: Checklist and tips

**Before go-live**

- [ ] API runs in Production mode (`ASPNETCORE_ENVIRONMENT=Production`).
- [ ] JWT secret is strong and only in env vars (not in repo).
- [ ] CORS allows only your SPA origin (no `*` in production if you can avoid it).
- [ ] Migrations applied on the hosted DB.
- [ ] React build uses the production API URL.

**After first deploy**

- [ ] Test register, login, and one full CRUD flow.
- [ ] Check API logs on Railway/Render for errors.
- [ ] (Optional) Add a custom domain in Vercel and Railway and use HTTPS (both provide it by default).

**If something breaks**

- **CORS error in browser:** SPA origin must be in `Cors:AllowedOrigins` and the API must call `UseCors()`.
- **401 on API calls:** Frontend must send `Authorization: Bearer <token>` and the token must be valid (same secret, not expired).
- **DB errors:** Connection string must be correct and the DB must have migrations applied; check Railway/Render DB logs.

---

## Summary

| Step | Where | What |
|------|--------|------|
| 1 | API code | Add CORS, use env vars for connection string and JWT. |
| 2 | Railway (or Render) | Create PostgreSQL + API service, set env vars, run migrations. |
| 3 | Vercel (or Netlify) | Deploy React build, set API URL env var. |
| 4 | Railway | Set CORS to your SPA URL, redeploy if needed. |
| 5 | Browser | Test register, login, CRUD. |

This gets you to a single deployed MVP: backend and frontend live, DB in the cloud, ready to extend (recurring transactions, budgets, etc.) later.
