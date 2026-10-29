namespace ApiFunkosConjunta.Infrastructure;

using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ApiFunkosConjunta.Repositories;
using ApiFunkosConjunta.Services;
using ApiFunkosConjunta.Validators;

public static class RepositoriesConfig
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        Log.Information("Registrando repositorios...");
        services.AddScoped<IFunkoRepository, FunkoRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        return services;
    }
}

public static class ServicesConfig
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        Log.Information("Registrando servicios...");
        services.AddScoped<IFunkoValidator, FunkoValidator>();
        services.AddScoped<IFunkoService, FunkoService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        return services;
    }
}
