using BankCore.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankCore.Infrastructure.Persistence.Configurations;

public sealed class TransactionConfiguration
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable(("Transactions"));
        
        builder.HasKey(t => t.Id);

        builder.Property(t => t.SourceAccountId)
            .IsRequired();

        builder.OwnsOne(t => t.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasPrecision(18, 2)
                .IsRequired();
            
            money.Property(m => m.Currency)
                .HasColumnName("Amount")
                /*
                    HasPrecision(18, 2) degine la precisión de un número decimal en la base de datos
                    18 = cantidad total de dígitod permitidos
                    2 = cuántos de esos dígitos van despues del punto decimal         
                */
                .HasPrecision(18, 2)
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(t => t.Type)
            .HasConversion<string>() // <- Para convertir un tipo enum a string en la db
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.FailureReason)
            .HasMaxLength(500);
        
        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.CompletedAt);

        builder.Ignore(t => t.DomainEvents);
    }
}