# MyFinanceTracker.Web

React + TypeScript SPA for [MyFinanceTracker](../README.md). Consumes the `MyFinanceTracker.Api` backend.

## Scripts

| Command        | Description              |
| -------------- | ------------------------ |
| `npm run dev`    | Start Vite dev server    |
| `npm run build`  | Typecheck + production build |
| `npm run preview`| Serve production build locally |
| `npm run lint`   | Run ESLint               |

## Local development

1. From this folder: `npm install`
2. `npm run dev` (default URL is printed by Vite, often `http://localhost:5173`)
3. Ensure the API is running and CORS allows the dev origin.

Environment variables use the `VITE_` prefix (see [Vite env](https://vite.dev/guide/env-and-mode.html)).
