# BankCore

REST API for banking operations built with .NET 10, Clean Architecture, DDD and CQRS.

## Tech Stack

- **.NET 10** / ASP.NET Core
- **Entity Framework Core 10** + SQL Server
- **MediatR 14** for CQRS
- **FluentValidation 12** for command validation
- **xUnit + FluentAssertions** for unit testing

## Architecture

```
BankCore/
├── src/
│   ├── BankCore.Domain          # Entities, value objects, domain events
│   ├── BankCore.Application     # Commands, Queries, Validators, Pipeline Behaviors
│   ├── BankCore.Infrastructure  # EF Core, repositories, configuration
│   └── BankCore.API             # Controllers, Program.cs
└── tests/
    └── BankCore.UnitTests       # Domain unit tests
```

Follows **Clean Architecture**: inner layers have no dependency on outer layers. The domain knows nothing about infrastructure or the API.

## Domain

The core entity is `Account`, modeled as an **Aggregate Root**:

- `Account.Open()` opens a new account with zero balance
- `Account.Credit()` credits money and raises `MoneyCredited`
- `Account.Debit()` debits money with balance validation and raises `MoneyDebited`
- `Account.Block()` / `Account.Close()` manage the account lifecycle

Monetary values are encapsulated in the **Value Object** `Money`, which prevents operations between different currencies and negative amounts.

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/accounts` | Open account |
| `GET` | `/api/accounts/{id}` | Get account by ID |
| `POST` | `/api/accounts/transfer` | Transfer money between accounts |

### Example: Open account

```http
POST /api/accounts
Content-Type: application/json

{
  "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "accountType": "Checking",
  "currency": "MXN"
}
```

Supported currencies: `MXN`, `USD`, `EUR`  
Account types: `Checking`, `Savings`

## Running locally

**Requirements:** .NET 10 SDK, SQL Server or LocalDB

```bash
# 1. Clone the repository
git clone https://github.com/edmorenodev/BankCore.git
cd BankCore

# 2. Apply migrations
dotnet ef database update --project src/BankCore.Infrastructure --startup-project src/BankCore.API

# 3. Run the API
dotnet run --project src/BankCore.API
```

API available at `http://localhost:5023`. Swagger at `/swagger`.

## Tests

```bash
dotnet test
```

19 unit tests covering the domain: `Account`, `Money` and domain events.

## Patterns applied

- **Clean Architecture** layered dependencies pointing inward
- **Domain-Driven Design** Aggregate Root, Value Object, Domain Events
- **CQRS** with MediatR, commands and queries separated
- **Repository + Unit of Work** persistence abstraction
- **Pipeline Behaviors** cross-cutting logging and validation without polluting handlers
