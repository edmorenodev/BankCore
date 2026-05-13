using BankCore.Domain.Accounts;
using BankCore.Domain.Shared;
using BankCore.Infrastructure.Persistence.Context;
using BankCore.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankCore.Infrastructure;

// Clase static no puede ser instanciada, clase sealed no puede ser heredada
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        /*
            Cuando hago services.Add estoy registrando en el contenedor de DI de ASP.NET Core que cuando
            usa clase solicite IAccountRepository, se le inyectará una instancia de AccountRepository.
            
            En DI e IoC se busca evitar el new poeque si creo objetos manualmente acoplo todo (clases dependen
            directamente de implementaciones).

            Usar DI y no new permite control + desacoplamiento + testabilidad + flexibilidad
        */
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        return services;
    }
}