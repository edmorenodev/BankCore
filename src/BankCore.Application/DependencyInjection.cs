using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BankCore.Application;

public class DependencyInjection
{
    public static IServiceCollection AddApplication(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }
}