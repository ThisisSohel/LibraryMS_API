# Library Management System — Backend API

A RESTful backend for a multi-branch **Library Management System**, built for the Software Engineer (.NET) technical assessment. Implements Authentication (JWT) & role-based authorization, Branch Management, Book Management, Member Management, Borrow & Return, Reservation Queue, and Reports, using Clean/Onion Architecture, CQRS (MediatR), and PostgreSQL.

The frontend (React/Angular/Vue/Blazor) is a separate, upcoming piece of work and is not included in this submission yet.

## Tech stack

- ASP.NET Core 8 (Web API)
- Entity Framework Core 8 + Npgsql (PostgreSQL), Code-First migrations, snake_case naming convention
- MediatR (CQRS command/query dispatch)
- FluentValidation (request validation, run automatically via a MediatR pipeline behaviour)
- Serilog (structured logging — console + rolling daily file)
- JWT Bearer authentication + role-based authorization (`Microsoft.AspNetCore.Identity.PasswordHasher` for password hashing)
- Swagger / OpenAPI (Swashbuckle), with a Bearer-token button wired up for testing protected endpoints
- Centralized exception-handling middleware (no per-endpoint try/catch)
- Optimistic concurrency on book-copy stock, using Postgres's `xmin` system column as the EF Core row-version token
- ClosedXML (Excel) and QuestPDF (PDF) for report exports

## Project structure

Clean/Onion Architecture, one project per layer:

```
LibraryManagementSystem.Domain          — entities, enums, no dependencies
LibraryManagementSystem.Application     — CQRS commands/queries/handlers, validators, DTOs,
                                           repository interfaces (depends only on Domain)
LibraryManagementSystem.Infrastructure  — EF Core DbContext, repository implementations,
                                           JWT token generation, migrations
LibraryManagementSystem.API             — controllers, middleware, DI wiring, Program.cs
```

Each functional module (Books, Branches, Members, BorrowRecords, Reservations, Reports, Auth) follows the same shape inside `Application`: `XCommand` / `XQuery` records implementing `IRequest<T>`, a `FluentValidation` validator per command/query, and a handler that talks to a small repository interface (defined in `Application/Common/Interfaces`, implemented in `Infrastructure/Persistence/Repositories`).

## Setup instructions

### Prerequisites

- .NET 8 SDK
- PostgreSQL (running locally or reachable over the network)
- The `dotnet-ef` tool, restored via the repo's local tool manifest (see below)

### 1. Clone and restore

```bash
git clone <this-repo-url>
cd WebAPI
dotnet tool restore
dotnet restore
```

### 2. Configure environment

Copy the connection string and JWT placeholders from `LibraryManagementSystem.API/appsettings.json` into a new, git-ignored `LibraryManagementSystem.API/appsettings.Development.json` with your real local values:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=librarydb;Username=postgres;Password=<your-password>"
  },
  "Jwt": {
    "Secret": "<a long random string, at least 32 characters>",
    "Issuer": "LibraryManagementSystem",
    "Audience": "LibraryManagementSystem",
    "ExpiryMinutes": "60"
  }
}
```

`appsettings.Development.json` is in `.gitignore` — it is never committed. `appsettings.json` only ever holds non-functional placeholder values (`CHANGE_ME`).

### 3. Apply migrations

```bash
dotnet ef database update --project LibraryManagementSystem.Infrastructure --startup-project LibraryManagementSystem.API
```

This creates the schema and seeds two roles (`Admin`, `Librarian`) plus one bootstrap Admin user — see **Default credentials** below.

### 4. Run

```bash
dotnet run --project LibraryManagementSystem.API
```

Swagger UI opens automatically at `https://localhost:7002/swagger` (or `http://localhost:5179/swagger`). Use the **Authorize** button with a JWT from `POST /api/auth/login` to call protected endpoints directly from Swagger.

## How to run tests

Not included in this submission round — prioritized completing all seven functional modules plus authentication within the available time. The Application layer's handlers are structured to be testable in isolation (each depends only on small repository interfaces, easily mocked), so this is a natural next addition rather than a redesign.

## Default credentials

A single bootstrap Admin account is seeded via an EF Core migration, since staff accounts can only be created through `POST /api/auth/register`, which is itself Admin-only — there has to be one real account to start from.

| Username | Password    | Role  |
|----------|-------------|-------|
| `admin`  | `Admin@123` | Admin |

**Rotate this password immediately in any real deployment.** It exists only to bootstrap the system; use it to log in once and register real staff accounts.

## API overview

All endpoints except `POST /api/auth/login` require a `Bearer` JWT. Branch mutations (create/update/delete) and `POST /api/auth/register` additionally require the `Admin` role; everything else is available to both `Admin` and `Librarian`.

| Module | Base route | Notes |
|---|---|---|
| Auth | `/api/auth` | `login` (public), `register` (Admin-only), `me` |
| Branches | `/api/branches` | CRUD, search, pagination |
| Books | `/api/books` | CRUD, search, pagination; nested `GET/POST /{id}/copies` for per-branch stock |
| Members | `/api/members` | CRUD, search, branch filter, pagination |
| Borrow & Return | `/api/borrowrecords` | checkout, `POST /{id}/return`, filterable list |
| Reservations | `/api/reservations` | create, `POST /{id}/cancel`, `POST /{id}/fulfill`, filterable list |
| Reports | `/api/reports` | `overdue-books`, `most-borrowed-books`, `branch-inventory-summary` (read-only); each also has a `GET /{report}/export?format=xlsx\|pdf` variant |

Full request/response shapes are in Swagger.

## Assumptions & design decisions

The brief allows — and explicitly asks for — documenting reasonable assumptions where requirements are open-ended, rather than guessing silently. In the order they came up:

- **Members do not log in.** Only staff (`Users`, role `Admin` or `Librarian`) authenticate. Members are patron records managed by staff.
- **Book copies are tracked as a count per branch** (`total_copies` / `available_copies`), not as individually barcoded physical items.
- **Audit columns** (`created_by` / `updated_by`) are plain usernames, not FKs to `Users` — audit history survives even if the user account is later removed. No soft-delete or full change-history table.
- **Role split**: `Admin` manages system structure — branches and staff account provisioning. `Admin` and `Librarian` both handle day-to-day operations — books, members, borrow/return, reservations, reports. This wasn't specified in the brief; it's the split that matched how a real library's staff roles typically divide.
- **Staff registration is Admin-only**, not self-service — consistent with members also not self-registering. This creates a bootstrap requirement, solved by seeding one real Admin account (see **Default credentials**).
- **Delete guards over hard deletes**: Branches, Books, and Members cannot be deleted while other records reference them (staff, copies, borrow history, reservations) — the API returns `409 Conflict` with a message suggesting deactivation instead. This preserves referential and audit integrity rather than allowing silent data loss.
- **Reservations are only permitted when no copies are currently available** at the requested branch (checked against `book_copies.available_copies`); otherwise the API expects the member to borrow directly. Queue position is scoped per (book, branch) pair, assigned FIFO on creation, and shifted down automatically when an earlier reservation is cancelled or fulfilled. Fulfilling out of turn is rejected.
- **Overdue status is computed at read time** (`IsOverdue` on the borrow-record DTO, comparing `DueDate` to now) rather than persisted via a scheduled status transition, since Background Jobs are explicitly out of scope this round.
- **Loan period is a fixed 14 days**, not configurable — kept simple for this scope.
- **One active borrow per member per book**, and **one active reservation per member per book/branch**, enforced at the application layer.
- **`ProcessedByUserId`** on borrow/return and reservation-fulfillment actions is derived from the caller's JWT identity, never accepted from the request body — a client cannot attribute a transaction to a different staff user.
- **Pagination is bounded** (`pageSize` capped at 100) on every list endpoint, to prevent a single request from pulling an entire table.
- **Optimistic concurrency uses Postgres's `xmin` system column** as the EF Core concurrency token on `BookCopy`, rather than a hand-rolled `RowVersion` column — it needs no schema change (the column already exists on every table) and is the idiomatic approach on Postgres. Two concurrent borrow/return/fulfil/add-copy requests against the same (book, branch) stock row now produce one success and one `409 Conflict` ("This record was changed by another request. Please retry.") instead of a silently lost update.
- **Report exports are generated on-demand**, not cached or queued — each `GET /{report}/export` call re-runs the same query the list endpoint uses and streams the file back synchronously. Fine at this data scale; would need to move to a background job for very large exports.
- **QuestPDF's Community license** (free for organizations under $1M annual gross revenue) is used for PDF generation — set once at startup in `Program.cs`. Would need a commercial license for a business above that threshold.

### Explicitly out of scope this round

Per the brief's bonus list, only **CQRS, Domain Events, Optimistic Concurrency, Redis, and Excel/PDF Export** were selected as in-scope bonus features for this round; of those, **CQRS** (via MediatR, used throughout), **Optimistic Concurrency** (Postgres `xmin` on `BookCopy`), and **Excel/PDF Export** (Reports module) are implemented. **Domain Events** and **Redis** are not yet started. **API Versioning, Health Checks, Docker, Background Jobs, Email Notifications, and CI/CD Pipeline** were deliberately excluded this round to focus on a smaller set of features implemented well, rather than stretching thin across all of them. They may be added in a later iteration.

Unit tests were also deferred this round (see **How to run tests** above).
