# Finance Tracker – Database Schema Plan

## Overview

This document defines the tables and relationships for the MyFinanceTracker application. All finance data is scoped per user (multi-tenant by `UserId`).

---

| Table / schema | Description |
|----------------|-------------|
| **identity** (ASP.NET Identity) | Users, roles, logins. `ApplicationUser` has Id, Email, FirstName, LastName, EnableNotifications. |

---

### 1. **Account**

Represents where money is held (bank, cash, card, etc.).

| Column | Type | Notes |
|--------|------|--------|
| Id | Guid (PK) | |
| UserId | string (FK → Identity) | Owner; required |
| Name | string | e.g. "Main Bank", "Cash", "Credit Card" |
| AccountType | string / enum | e.g. Bank, Cash, CreditCard, Savings |
| Currency | enum (string) | `CurrencyCode`: USD, EUR (stored as "USD", "EUR") |
| Balance | decimal | Current balance |
| IsActive | bool | Soft visibility (default true) |
| CreatedAt | DateTime (UTC) | |
| UpdatedAt | DateTime (UTC) | |

**Indexes:** `UserId`, `UserId + IsActive`

---

### 2. **Category**

Income or expense category. User-specific so each user can define their own.

| Column | Type | Notes |
|--------|------|--------|
| Id | Guid (PK) | |
| UserId | string (FK → Identity) | Owner; required |
| Name | string | e.g. "Food", "Salary" |
| Type | string / enum | Income or Expense |
| IsActive | bool | Default true |
| CreatedAt | DateTime (UTC) | |
| UpdatedAt | DateTime (UTC) | |

**Indexes:** `UserId`, `UserId + Type`, `ParentCategoryId`

---

### 3. **Transaction**

Single income or expense entry.

| Column | Type | Notes |
|--------|------|--------|
| Id | Guid (PK) | |
| UserId | string (FK → Identity) | Owner; required |
| AccountId | Guid (FK → Account) | Where the money moved |
| CategoryId | Guid (FK → Category) | What it was for |
| Amount | decimal |
| TransactionDate | DateTime (UTC or date) | When it occurred |
| Description | string? | Optional description |
| CreatedAt | DateTime (UTC) | |
| UpdatedAt | DateTime (UTC) | |

**Indexes:** `UserId`, `AccountId`, `CategoryId`, `TransactionDate`, `UserId + TransactionDate`

**Convention:** Either store signed amounts (positive = income, negative = expense) or add a `Type` (Income/Expense) and store amount as positive. Recommend **signed amount** for simpler queries.

---

## Entity Relationship (high level)

```
ApplicationUser (Identity)
    │
    ├── 1:N → Account
    ├── 1:N → Category (ParentCategoryId → Category for hierarchy)
    └── 1:N → Transaction
                    │
                    ├── N:1 → Account
                    └── N:1 → Category
```

---

## Schema naming

- **Identity:** already in schema `identity`.
- **Finance tables:** use schema `finance` (or `app`) so `identity` and finance are clearly separated.
- **Table names:** `Account`, `Category`, `Transaction` (singular, matching entities).

---

## later phases

- **RecurringTransaction** – template for recurring income/expense (amount, frequency, next date, category, account).
- **Budget** – e.g. monthly budget per category (CategoryId, Month, Year, Amount).

---

## Summary

| Table | Purpose |
|-------|---------|
| **Account** | Where the user keeps money (bank, cash, card). |
| **Category** | What the money is for (income/expense, optional hierarchy). |
| **Transaction** | One income or expense row (amount, date, account, category). |

All three reference `UserId` for multi-tenant isolation. Next step is to add these entities, EF configurations, and migrations, then implement CRUD APIs (and optionally a minimal Categories + Accounts API so transactions can reference them).
