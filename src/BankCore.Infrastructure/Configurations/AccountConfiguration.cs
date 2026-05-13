using BankCore.Domain.Accounts;
using BankCore.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankCore.Infrastructure.Persistence.Configurations;

public sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AccountNumber)
            .IsRequired() // <- IsRequired() equivale en SQL a NOT NULL
            .HasMaxLength(50);

        builder.HasIndex(a => a.AccountNumber)
            .IsUnique();

        builder.Property(a => a.OwnerId)
            .IsRequired();

        builder.Property(a => a.Type)
            .HasConversion<string>() // <- Para convertir un tipo enum en un string en la db
            .IsRequired();

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .IsRequired();

        // Money como owned entity, hace que Money se mapee a una columna de la tabla Account
        builder.OwnsOne(a => a.Balance, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Balance") // Literalmente significa "Money.Amount -> Columna Balance"
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(a => a.OpenedAt)
            .IsRequired();

        builder.Property(a => a.LastMovementAt);

        builder.Property(a => a.Version)
            .IsRowVersion();

        // Ignorar los domain events porque no se guardan en esta tabla
        builder.Ignore(a => a.DomainEvents);
    }
}