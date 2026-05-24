# MyFinanceTracker.Web

React + TypeScript SPA for [MyFinanceTracker](../README.md). Consumes the `MyFinanceTracker.Api` backend.

## Tech stack

- **Build**: Vite + React 19 + TypeScript (strict, `erasableSyntaxOnly`)
- **Styling**: Tailwind CSS v4 (`@tailwindcss/vite`) with the shadcn/ui `radix-nova` style
- **UI**: shadcn/ui (Radix UI primitives) in `src/components/ui/`, icons from `lucide-react`
- **Routing**: `react-router` v7 (data router via `createBrowserRouter`) with lazy-loaded feature pages
- **Server state**: `@tanstack/react-query` v5 + devtools
- **Forms**: `react-hook-form` + `zod` + `@hookform/resolvers` via shadcn's `<Form>` primitives
- **Auth**: React Context + `localStorage`-backed JWT; 401s dispatch a `window` event the auth provider listens to
- **Toasts**: `sonner` (via shadcn)
- **Dates**: `date-fns` + shadcn `Calendar`/`Popover` DatePicker

## Scripts

| Command           | Description                            |
| ----------------- | -------------------------------------- |
| `npm run dev`     | Start the Vite dev server on port 3000 |
| `npm run build`   | Typecheck + production build to `dist` |
| `npm run preview` | Serve the production build locally     |
| `npm run lint`    | Run ESLint                             |

## Local development

1. From this folder: `npm install`.
2. Run the API (`MyFinanceTracker.Api`) on `http://localhost:5010` (the `http` launch profile).
3. `npm run dev` (opens on `http://localhost:3000`).

The Vite dev server proxies `/api/*` to `http://localhost:5010`, so the SPA uses relative URLs and there is no CORS in development. In production, set `VITE_API_BASE_URL` in `.env.production` to the API origin and add that origin to `Cors:AllowedOrigins` in the API's `appsettings.json`.

## Project layout

```
src/
  app/                       composition root: AppProviders, router
  components/ui/             shadcn/ui components (edit freely)
  lib/utils.ts               cn() helper
  shared/                    cross-cutting building blocks
    api/                     fetch wrapper, ApiError, ProblemDetails
    auth/                    AuthContext, useAuth, ProtectedRoute, tokenStorage
    enums/                   AccountType / CurrencyCode / TransactionType (numeric, as-const)
    forms/                   DatePicker, Combobox, mapServerErrors
    components/              AppLayout, AuthLayout, PageHeader, AsyncBoundary, ConfirmDialog, ThemeToggle
    utils/                   money / date helpers
    queryClient.ts           TanStack Query client + defaults
  features/                  vertical slices (mirror API Features/)
    auth/                    login / register
    accounts/                CRUD + table + dialog
    categories/              CRUD + table + dialog
    transactions/            CRUD + URL-driven filters + table + dialog
    dashboard/               composes the other features' hooks
    errors/                  404
  main.tsx                   bootstraps <AppProviders />
  index.css                  Tailwind v4 entry + shadcn theme variables
```

## Architectural conventions

- **Vertical slices**: each feature owns its `types.ts`, `schemas.ts` (zod), `api.ts` (TanStack Query hooks with a key factory), `components/`, and `pages/`.
- **Dependencies**: `features/*` may depend on `shared/*` and `components/ui/*` but never on each other (no cross-feature imports).
- **Numeric enums**: the API serializes `AccountType` / `CurrencyCode` / `TransactionType` as numbers. We mirror them with `as const` objects (TS `enum` is forbidden by `erasableSyntaxOnly`).
- **Validation**: zod schemas validate on the client; the API's `ValidationProblemDetails` is mapped back into RHF errors via `shared/forms/mapServerErrors.ts`.
- **Cache invalidation**: each feature exposes a `keys` factory (`accountsKeys`, `categoriesKeys`, `transactionsKeys`). Mutations invalidate the right key prefix on success.
- **401 handling**: the api client clears the in-memory token and dispatches `auth:unauthorized`; the AuthContext listens for it and signs the user out.

## Environment variables

Use the `VITE_` prefix (see [Vite env](https://vite.dev/guide/env-and-mode.html)).

| Variable            | Description                                                                              |
| ------------------- | ---------------------------------------------------------------------------------------- |
| `VITE_API_BASE_URL` | Base URL for API calls. Empty in dev (relative URLs via Vite proxy); set for production. |
