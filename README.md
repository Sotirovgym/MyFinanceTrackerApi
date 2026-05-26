# MyFinanceTracker

A personal finance tracker. ASP.NET Core 9 backend with JWT auth + EF Core, and a React 19 + Vite + TypeScript SPA on top.

## Projects

| Project                         | What it is                                                           |
| ------------------------------- | -------------------------------------------------------------------- |
| `MyFinanceTracker.Api`          | ASP.NET Core Web API (JWT, controllers, Swagger, Serilog)            |
| `MyFinanceTracker.Application`  | Application services, DTOs, FluentValidation validators              |
| `MyFinanceTracker.Core`         | Domain entities and enums                                            |
| `MyFinanceTracker.Infrastructure` | EF Core `DbContext`, repositories, Identity, migrations            |
| `MyFinanceTracker.Web`          | React + Vite + TypeScript SPA (Tailwind v4 + shadcn/ui)              |

The backend follows clean architecture conventions documented in [`.cursor/rules/project-standards.mdc`](.cursor/rules/project-standards.mdc).

## Run locally end-to-end

You need .NET 9 SDK and Node.js 20+.

1. **Configure secrets for the API** (one-time): set `ConnectionStrings:DefaultConnection` and `Jwt:Secret` via user secrets or `appsettings.Development.json` (see [hosting-guide.md](docs/hosting-guide.md) for details).

2. **Start the API** (from the repo root):

   ```bash
   dotnet run --project MyFinanceTracker.Api
   ```

   The HTTP profile listens on `http://localhost:5010` (and `https://localhost:7207`).

3. **Start the SPA** in a second terminal:

   ```bash
   cd MyFinanceTracker.Web
   npm install        # first time only
   npm run dev
   ```

4. **Open** <http://localhost:3000>.

The Vite dev server proxies `/api/*` to `http://localhost:5010`, so the SPA talks to one origin and there's no CORS in development.

> **TL;DR**: `dotnet run --project MyFinanceTracker.Api` &nbsp;then&nbsp; `cd MyFinanceTracker.Web && npm run dev` &nbsp;then open&nbsp; <http://localhost:3000>.

## Documentation

| Doc                                                            | What's in it                                                                   |
| -------------------------------------------------------------- | ------------------------------------------------------------------------------ |
| [docs/project-plan.md](docs/project-plan.md)                   | Backend roadmap (completed scope + planned next steps)                         |
| [docs/database-schema-plan.md](docs/database-schema-plan.md)   | Database schema design                                                         |
| [docs/hosting-guide.md](docs/hosting-guide.md)                 | Hosting / configuration / deployment guide                                     |
| [docs/web-spa-plan.md](docs/web-spa-plan.md)                   | The design plan for the React SPA (tech choices, architecture, step-by-step)   |
| [docs/web-spa-implementation.md](docs/web-spa-implementation.md) | What was actually built in `MyFinanceTracker.Web`, file map, conventions, gotchas |
| [docs/web-spa-learning-guide.md](docs/web-spa-learning-guide.md) | **New to React?** Reading order, .NET analogies, exercises for this codebase   |
| [MyFinanceTracker.Web/README.md](MyFinanceTracker.Web/README.md) | SPA-local README (scripts, env vars, layout)                                  |

## Useful API endpoints

The API exposes:

- `POST /api/Auth/register`, `POST /api/Auth/login`
- CRUD: `/api/Accounts`, `/api/Categories`, `/api/Transactions`
- `GET /api/Transactions` accepts query parameters: `accountId`, `categoryId`, `from`, `to` (ISO 8601).
- Swagger UI is available in Development at `/swagger`.

See [MyFinanceTracker.Api.http](MyFinanceTracker.Api/MyFinanceTracker.Api.http) for ready-to-run request examples.

## License

See [LICENSE.txt](LICENSE.txt).
