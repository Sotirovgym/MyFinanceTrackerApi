# MyFinanceTracker.Web - Learning Guide for React Beginners

You already have a working app. The fastest way to learn React here is to read **this codebase** in a deliberate order, not random files. Think of it like learning a new .NET solution: start at the entry point, then middleware/providers, then one feature end-to-end.

For architecture and file locations, see [web-spa-implementation.md](web-spa-implementation.md). For how to run the app locally, see the root [README.md](../README.md).

---

## Map React concepts to what you already know

| .NET / familiar idea        | In this project |
| --------------------------- | --------------- |
| `Program.cs` + DI           | [`main.tsx`](../MyFinanceTracker.Web/src/main.tsx) → [`AppProviders.tsx`](../MyFinanceTracker.Web/src/app/AppProviders.tsx) |
| Middleware pipeline         | Providers wrap the app (Query, Auth, Router) |
| Controllers + routes        | [`router.tsx`](../MyFinanceTracker.Web/src/app/router.tsx) |
| DTOs                        | `features/*/types.ts` |
| FluentValidation            | `features/*/schemas.ts` (Zod) |
| Application services        | `features/*/api.ts` (TanStack Query hooks) |
| Razor views / Blazor pages  | `features/*/pages/*.tsx` |
| Partial views / components  | `features/*/components/*.tsx` |
| `HttpClient` + JWT handler  | [`shared/api/client.ts`](../MyFinanceTracker.Web/src/shared/api/client.ts) |
| `IHttpContextAccessor` / claims | [`AuthContext`](../MyFinanceTracker.Web/src/shared/auth/AuthContext.tsx) + [`useAuth`](../MyFinanceTracker.Web/src/shared/auth/useAuth.ts) |

React is **UI = function of state**. When state changes, the component function runs again and the DOM updates. There is no separate "code-behind" file — logic and markup live together in `.tsx` files.

---

## Recommended reading order (about 2–3 hours)

### Phase 1 — How the app boots (15 min)

1. [`src/main.tsx`](../MyFinanceTracker.Web/src/main.tsx) — mounts React into `#root` in `index.html`.
2. [`src/app/AppProviders.tsx`](../MyFinanceTracker.Web/src/app/AppProviders.tsx) — the composition root: who wraps whom.
3. [`src/app/router.tsx`](../MyFinanceTracker.Web/src/app/router.tsx) — which URL shows which page; notice `ProtectedRoute` and `lazy()`.

**Question to answer:** What happens when you open `/accounts` while logged out?

---

### Phase 2 — One simple page (30 min)

4. [`src/features/auth/pages/LoginPage.tsx`](../MyFinanceTracker.Web/src/features/auth/pages/LoginPage.tsx) — a full page in one file:
   - `useForm` + Zod = client validation
   - `useLogin()` = API call (mutation)
   - `signIn()` = save token
   - `navigate()` = redirect after success

5. [`src/shared/auth/AuthContext.tsx`](../MyFinanceTracker.Web/src/shared/auth/AuthContext.tsx) — global auth state (like a scoped service).
6. [`src/shared/auth/ProtectedRoute.tsx`](../MyFinanceTracker.Web/src/shared/auth/ProtectedRoute.tsx) — route guard.

**Question to answer:** Where is the JWT stored, and how does it get onto API requests?

---

### Phase 3 — How data reaches the API (30 min)

7. [`src/shared/api/client.ts`](../MyFinanceTracker.Web/src/shared/api/client.ts) — `fetch` wrapper, bearer token, 401 handling.
8. [`src/features/accounts/api.ts`](../MyFinanceTracker.Web/src/features/accounts/api.ts) — `useAccounts`, `useCreateAccount`, cache keys.

**Question to answer:** After you create an account, why does the list refresh without a full page reload?

---

### Phase 4 — One full feature slice (45 min) — most important

Read **Accounts** top to bottom; every other feature copies this pattern:

| Order | File | Role |
| ----- | ---- | ---- |
| 1 | [`types.ts`](../MyFinanceTracker.Web/src/features/accounts/types.ts) | Shape of API data |
| 2 | [`schemas.ts`](../MyFinanceTracker.Web/src/features/accounts/schemas.ts) | Form validation rules |
| 3 | [`api.ts`](../MyFinanceTracker.Web/src/features/accounts/api.ts) | Load/save/delete hooks |
| 4 | [`pages/AccountsPage.tsx`](../MyFinanceTracker.Web/src/features/accounts/pages/AccountsPage.tsx) | Orchestrates UI + state |
| 5 | [`components/AccountsTable.tsx`](../MyFinanceTracker.Web/src/features/accounts/components/AccountsTable.tsx) | Present data |
| 6 | [`components/AccountFormDialog.tsx`](../MyFinanceTracker.Web/src/features/accounts/components/AccountFormDialog.tsx) | Create/edit form |

**Question to answer:** What does `useState` control on `AccountsPage`? (dialogs, selection, delete target)

Then skim **Categories** — same shape, faster read.

---

### Phase 5 — The harder feature (30 min)

9. [`src/features/transactions/useTransactionFilters.ts`](../MyFinanceTracker.Web/src/features/transactions/useTransactionFilters.ts) — filters in the URL (`useSearchParams`).
10. [`TransactionFilters.tsx`](../MyFinanceTracker.Web/src/features/transactions/components/TransactionFilters.tsx) + [`TransactionsPage.tsx`](../MyFinanceTracker.Web/src/features/transactions/pages/TransactionsPage.tsx).

**Question to answer:** If you refresh the page with filters in the URL, why do they stay?

---

### Phase 6 — Layout and shared UI (20 min)

- [`AppLayout.tsx`](../MyFinanceTracker.Web/src/shared/components/AppLayout.tsx) — sidebar, mobile menu.
- [`AsyncBoundary.tsx`](../MyFinanceTracker.Web/src/shared/components/AsyncBoundary.tsx) — loading / error / empty.
- Pick any file in [`src/components/ui/`](../MyFinanceTracker.Web/src/components/ui/) — shadcn building blocks (Button, Dialog, Table). You **use** these; you rarely need to understand every line inside them at first.

---

## Core React ideas (learn these while reading)

You only need a handful of concepts to read 90% of this codebase:

1. **Components** — functions that return JSX (`export default function AccountsPage() { return (...); }`).
2. **Props** — arguments to a component (`<AccountsTable accounts={sorted} onEdit={handleEdit} />`).
3. **`useState`** — local UI state (dialog open, selected row).
4. **`useEffect`** — run code when something changes (sync form when dialog opens, listen for 401 event).
5. **`useMemo`** — derived data (sorted list) without recomputing every render.
6. **Custom hooks** — `useAccounts`, `useAuth` — reusable logic with a `use` prefix.
7. **Context** — pass auth down without prop-drilling every level.

You do **not** need class components, Redux, or advanced patterns for this app.

---

## Libraries used here (learn on demand)

| Library | Learn when you read… | Official docs |
| ------- | -------------------- | ------------- |
| **React** | Any `.tsx` file | [react.dev/learn](https://react.dev/learn) |
| **React Router** | `router.tsx`, `LoginPage` (`useNavigate`) | [reactrouter.com](https://reactrouter.com) |
| **TanStack Query** | `features/*/api.ts` | [TanStack Query - React](https://tanstack.com/query/latest/docs/framework/react/overview) |
| **React Hook Form + Zod** | `LoginPage`, `AccountFormDialog` | [react-hook-form.com](https://react-hook-form.com), [zod.dev](https://zod.dev) |
| **Tailwind** | `className="..."` on elements | [tailwindcss.com/docs](https://tailwindcss.com/docs) |
| **shadcn/ui** | `components/ui/*` | [ui.shadcn.com](https://ui.shadcn.com) |

For a .NET developer, **React's own tutorial** ([react.dev/learn](https://react.dev/learn)) chapters 1–5 plus "Managing state" are enough before diving deeper into TanStack Query.

---

## Hands-on exercises (do these in the repo)

1. **Trace a click** — On Accounts, click "New account". Follow: button → `setFormOpen(true)` → `AccountFormDialog` → submit → `useCreateAccount` → `api.post` → toast → dialog closes → list refetches.

2. **Change copy** — Change the Accounts page title in `PageHeader` and confirm hot reload updates the browser.

3. **Add a column** — In `AccountsTable`, show `createdAt` formatted with `formatDate` from `shared/utils/date.ts`.

4. **Break and fix** — Temporarily remove `signIn` after login; see what breaks; restore it.

5. **Use DevTools** — With `npm run dev`, open React Query Devtools (bottom-right) and watch cache keys when you create/delete an account.

---

## Related project docs

| Doc | What's in it |
| --- | ------------ |
| [web-spa-implementation.md](web-spa-implementation.md) | File map, request flow, conventions, gotchas |
| [web-spa-plan.md](web-spa-plan.md) | Original design plan and tech choices |
| [MyFinanceTracker.Web/README.md](../MyFinanceTracker.Web/README.md) | Scripts, env vars, folder layout |

---

## Suggested pace

| Week | Focus |
| ---- | ----- |
| 1 | Phases 1–2 + react.dev "Describing the UI" and "Adding interactivity" |
| 2 | Phase 3–4 (Accounts slice) + TanStack Query "Queries" doc |
| 3 | Transactions + forms (RHF + Zod) |
| 4 | Small change of your own (e.g. dashboard card, new filter) |

---

## Bottom line

Start at [`main.tsx`](../MyFinanceTracker.Web/src/main.tsx) → [`router.tsx`](../MyFinanceTracker.Web/src/app/router.tsx) → [`LoginPage.tsx`](../MyFinanceTracker.Web/src/features/auth/pages/LoginPage.tsx) → the **Accounts** folder. Once that slice clicks, Categories and Transactions are mostly repetition with extra UI (combobox, date picker, URL filters).
