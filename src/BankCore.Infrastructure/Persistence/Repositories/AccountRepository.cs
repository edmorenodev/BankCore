using BankCore.Domain.Accounts;
using BankCore.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BankCore.Infrastructure.Persistence.Repositories;
/*
    El repositorio accede a la db a través del contexto para establecer como interactuar con los datos
*/
public sealed class AccountRepository : IAccountRepository
{
    // Se declara un campo privado de solo lectura para que acceda al contexto de la base de datos y utilizarlo
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Aquí esta utilizando el contexto y accediendo a la tabla "Accounts"
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts
            // Busca en la base de datos la primera cuenta cuyo AccountNumber coincida con el valor recibido como parámetro
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, cancellationToken);
    }

    public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
    {
        await _context.Accounts.AddAsync(account, cancellationToken);
    }

    public async Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
    {
        _context.Accounts.Update(account);
    }
}