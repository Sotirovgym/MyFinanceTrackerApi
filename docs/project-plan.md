# MyFinanceTracker – Project Plan

## Completed (initial scope)

| Step | Description | Status |
|------|-------------|--------|
| 1 | **JWT authentication** – Register, Login, bearer token, roles | Done |
| 2 | **Validation** – FluentValidation for auth and CRUD requests, filter validation | Done |
| 3 | **CRUD for main tables** – Accounts, Categories, Transactions (list with filters) | Done |

Also in place: global exception handling, Swagger, Serilog, Clean Architecture, repository + UoW, Result pattern, multi-tenant by `UserId`, public schema + lowercase/snake_case for finance tables.

---

## Next steps (recommended order)

### High priority (core product & robustness)

| Step | Description | Why |
|------|-------------|-----|
| 4 | **Recurring transactions** | Core feature: templates (amount, frequency, next date, category, account) and a job/process that creates actual transactions. |
| 5 | **Budgets** | Core feature: e.g. monthly budget per category (category, month, year, amount) and comparison to actual spending. |
| 6 | **Tests** | No test project yet. Add unit tests (services, validators) and integration tests (API) so changes are safe. |
| 7 | **Refresh token** | Only JWT access token today. Add refresh token (e.g. in DB or cookie) for longer sessions without re-login. |

### Medium priority (security & production readiness)

| Step | Description | Why |
|------|-------------|-----|
| 8 | **Password reset / Forgot password** | Users need a way to reset password (email link or token). |
| 9 | **Rate limiting** | Protect login/register and API from brute force and abuse. |
| 10 | **Health checks** | `/health` (and e.g. `/health/ready` with DB check) for hosting and monitoring. |
| 11 | **CORS** | Explicit CORS policy if a SPA or mobile app will call the API from another origin. |

### Later (premium & polish)

| Step | Description | Why |
|------|-------------|-----|
| 12 | **Subscription / premium** | Gate recurring transactions and budgets behind a paid plan (Stripe or store IAP), plus entitlement checks in API. |
| 13 | **Email confirmation** | Optional: require confirmed email before full access; needs email sending (e.g. SendGrid). |
| 14 | **README & run instructions** | Document how to run, migrate DB, and (optionally) deploy. |
| 15 | **Docker / deployment** | Dockerfile and optional compose for local and production deployment. |

---

## Summary

- **Done:** JWT (1), Validation (2), CRUD for Accounts/Categories/Transactions (3).
- **Next:** Recurring transactions (4), Budgets (5), Tests (6), Refresh token (7), then password reset, rate limiting, health checks, CORS, and later premium/subscription and docs/deploy.

This file can be updated as steps are completed or priorities change.
