# BankCore

API REST bancaria construida en .NET 10 con Clean Architecture, DDD y CQRS. Proyecto de práctica para demostrar el diseño de sistemas orientados al dominio.

## Tecnologías

- **.NET 10** / ASP.NET Core
- **Entity Framework Core 10** + SQL Server
- **MediatR 14** — implementación de CQRS
- **FluentValidation 12** — validación de comandos
- **xUnit + FluentAssertions** — pruebas unitarias

## Arquitectura

```
BankCore/
├── src/
│   ├── BankCore.Domain          # Entidades, value objects, eventos de dominio
│   ├── BankCore.Application     # Commands, Queries, Validators, Pipeline Behaviors
│   ├── BankCore.Infrastructure  # EF Core, repositorios, configuración
│   └── BankCore.API             # Controllers, Program.cs
└── tests/
    └── BankCore.UnitTests       # Pruebas unitarias del dominio
```

El proyecto sigue **Clean Architecture**: las capas internas no dependen de las externas. El dominio no conoce nada de infraestructura ni de la API.

## Dominio

La entidad central es `Account` (cuenta bancaria), modelada como **Aggregate Root**:

- `Account.Open()` — abre una cuenta nueva con saldo cero
- `Account.Credit()` — acredita dinero y emite `MoneyCredited`
- `Account.Debit()` — debita dinero con validación de saldo y emite `MoneyDebited`
- `Account.Block()` / `Account.Close()` — gestión del ciclo de vida

El valor monetario está encapsulado en el **Value Object** `Money`, que previene operaciones entre monedas distintas y montos negativos.

## Endpoints

| Método | Ruta | Descripción |
|--------|------|-------------|
| `POST` | `/api/accounts` | Abrir cuenta |
| `GET` | `/api/accounts/{id}` | Consultar cuenta por ID |
| `POST` | `/api/accounts/transfer` | Transferir dinero entre cuentas |

### Ejemplo — Abrir cuenta

```http
POST /api/accounts
Content-Type: application/json

{
  "ownerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "accountType": "Checking",
  "currency": "MXN"
}
```

Monedas soportadas: `MXN`, `USD`, `EUR`  
Tipos de cuenta: `Checking` (débito), `Savings` (ahorro)

## Ejecutar localmente

**Requisitos:** .NET 10 SDK, SQL Server o LocalDB

```bash
# 1. Clonar el repositorio
git clone https://github.com/edmorenodev/BankCore.git
cd BankCore

# 2. Aplicar migraciones
dotnet ef database update --project src/BankCore.Infrastructure --startup-project src/BankCore.API

# 3. Ejecutar la API
dotnet run --project src/BankCore.API
```

La API queda disponible en `http://localhost:5023`. Swagger en `/swagger`.

## Pruebas

```bash
dotnet test
```

19 pruebas unitarias sobre el dominio: `Account`, `Money` y eventos de dominio.

## Patrones aplicados

- **Clean Architecture** — separación en capas con dependencias hacia adentro
- **Domain-Driven Design** — Aggregate Root, Value Object, Domain Events
- **CQRS** con MediatR — comandos y queries separados
- **Repository + Unit of Work** — abstracción de persistencia
- **Pipeline Behaviors** — logging y validación transversal sin contaminar los handlers
