namespace BankCore.Application.Accounts.Queries.GetAccountById;
/*
    Esto es un DTO de salida, para que el cliente no reciba el aggregate completo.
    No se llama DTO porque en CQRS el nombre ya implica que es un DTO, porque un Response por definición
    solo transfiere datos, nunca tiene lógica.

    AccountDto -> genérico, puede usarse en varios contextos. DTO es el concepto general
    AccountResponse -> específico, es la respuesta de una Query
*/
public sealed record AccountResponse(
    Guid Id,
    string AccountNumber,
    Guid OwnerId,
    string Type,
    string Status,
    decimal Balance,
    string Currency,
    DateTime OpenedAt,
    DateTime? LastMovementAt
);