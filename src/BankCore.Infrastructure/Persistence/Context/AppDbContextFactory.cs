using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BankCore.Infrastructure.Persistence.Context;

// Permite a las herramientas de EF Core (dotnet ef migrations) crear el DbContext
// sin necesitar el host completo de ASP.NET Core en tiempo de diseño.
public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BankCoreDb;Trusted_Connection=True;")
            .Options;

        return new AppDbContext(options);
    }
}
