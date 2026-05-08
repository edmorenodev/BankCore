namespace BankCore.Domain.Shared;

public interface IDomainEventEntity
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}