# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Dedalo is a multi-tenant CMS (Content Management System) with a .NET 8 backend API. Manages websites, pages, menus, and content blocks with per-tenant database isolation.

## Commands

```bash
# Build
dotnet build Dedalo.sln

# Run API (https://localhost:44374)
dotnet run --project Dedalo.API

# Run all tests
dotnet test Dedalo.Tests/Dedalo.Tests.csproj

# Run a single test by name
dotnet test Dedalo.Tests/Dedalo.Tests.csproj --filter "FullyQualifiedName~WebsiteServiceTests.GetByIdAsync_ReturnsModel"

# Run tests in a class
dotnet test Dedalo.Tests/Dedalo.Tests.csproj --filter "FullyQualifiedName~WebsiteServiceTests"

# EF Core migrations
dotnet ef migrations add <MigrationName> --project Dedalo.Infra --startup-project Dedalo.API
dotnet ef database update --project Dedalo.Infra --startup-project Dedalo.API

# Docker
docker-compose up        # API + PostgreSQL 17
```

## Architecture

### Clean Architecture (.NET 8)

```
Dedalo.API              → Controllers, Startup, Middleware, Filters
Dedalo.Application      → DI bootstrap (ConfigureDedalo), Tenant resolution, DbContext factory
Dedalo.Domain           → Models, Services, Interfaces (business logic)
Dedalo.DTO              → Data transfer objects, Enums (zero dependencies)
Dedalo.Infra.Interfaces → Generic repository interfaces (no domain references)
Dedalo.Infra            → EF Core 9 DbContext, Repositories, AutoMapper profiles, Migrations
Dedalo.Tests            → xUnit + Moq
```

**Dependency flow:** API → Application → Domain → Infra.Interfaces ← Infra → PostgreSQL

### Multi-Tenant

Requests include `X-Tenant-Id` header → `TenantMiddleware` extracts it → `TenantResolver` resolves connection string/JWT/bucket from `appsettings.json` (`Tenants:{tenantId}:*`) → `TenantDbContextFactory` creates `DedaloContext` with tenant-specific connection. Falls back to `Tenant:DefaultTenantId` if header missing.

### Authentication

NAuth package handles Bearer token auth with per-tenant JWT secrets via `NAuthTenantSecretProvider`. Controllers get user via `_userClient.GetUserInSession(HttpContext)` which returns `UserSessionInfo` (synchronous, not async). User ID accessed as `user.UserId`.

### Entity Lifecycle Pattern

Every entity follows this exact pattern across layers:

1. **DTO** (`Dedalo.DTO/{Entity}/`): `{Entity}Info`, `{Entity}InsertInfo`, `{Entity}UpdateInfo` — all with `[JsonPropertyName("camelCase")]`
2. **Repository Interface** (`Dedalo.Infra.Interfaces/Repository/`): `I{Entity}Repository<TModel> where TModel : class` — always generic, never references Domain
3. **Domain Model** (`Dedalo.Domain/Models/`): `{Entity}Model` with `MarkCreated()`, `MarkUpdated()` methods
4. **Service Interface** (`Dedalo.Domain/Interfaces/`): `I{Entity}Service`
5. **Service** (`Dedalo.Domain/Services/`): `{Entity}Service` — validates ownership via parent Website, uses AutoMapper
6. **EF Entity** (`Dedalo.Infra/Context/`): plain POCO matching DB columns, enums stored as `int`, nav props as `virtual`
7. **Repository** (`Dedalo.Infra/Repository/`): maps Entity↔Model via AutoMapper, uses `DedaloContext`
8. **AutoMapper Profile** (`Dedalo.Infra/Mappers/`): 3 mapping directions — Entity↔Model (enum conversion), DTO→Model (ignore system fields), Model→DTO

### Ownership Model

All mutations validate ownership through the Website entity. Child entities (Page, Menu, Content) resolve their parent Website and call `website.ValidateOwnership(userId)`. This throws `UnauthorizedAccessException` on mismatch.

### REST API Structure

Controllers use nested RESTful routes reflecting entity hierarchy:
- `/website` — Website CRUD
- `/website/{websiteId}/page` — Page CRUD
- `/website/{websiteId}/menu` — Menu CRUD
- `/website/{websiteId}/page/{pageId}/content` — Content CRUD
- `/image` — Image upload via zTools IFileClient

Public (anonymous) endpoints use absolute routes:
- `GET /website/slug/{slug}` — Website by slug
- `GET /website/domain/{domain}` — Website by custom domain
- `GET /page/{pageSlug}?websiteSlug=x` or `?domain=x` — Page with grouped contents
- `GET /menu?websiteSlug=x` or `?domain=x` — Menu list
- `GET /content/{pageSlug}?websiteSlug=x` or `?domain=x` — Content list

### Content Area Pattern

`PUT /website/{id}/page/{id}/content/area` saves a batch of contents sharing the same `contentSlug`. The service diffs incoming items against DB: inserts new (contentId=0), updates existing, deletes removed.

## Database Conventions

- Tables: `dedalo_{plural_snake}` (e.g. `dedalo_websites`, `dedalo_pages`)
- Columns: `snake_case` (e.g. `website_slug`, `created_at`)
- PK: `dedalo_{table}_pkey`
- FK: `fk_dedalo_{child}_{parent}`
- Indexes: `ix_dedalo_{table}_{column}`
- Timestamps: `timestamp without time zone`, set via `DateTime.Now`
- Enums: stored as `int` with `HasDefaultValue(1)`

## DI Registration

All wiring in `Dedalo.Application/Startup.cs` via `ConfigureDedalo()`. Uses `injectDependency()` helper for scoped/transient registration. AutoMapper profiles registered by type. External clients: IUserClient, IFileClient, IChatGPTClient, IMailClient, IStringClient, IDocumentClient (from NAuth/zTools packages).

## Test Conventions

- xUnit + Moq, organized at `Dedalo.Tests/Domain/Services/` and `Dedalo.Tests/Domain/Mappers/`
- Service tests mock repositories and IMapper, verify business logic and ownership validation
- Mapper tests use real AutoMapper config, validate all mapping directions and `AssertConfigurationIsValid()`
