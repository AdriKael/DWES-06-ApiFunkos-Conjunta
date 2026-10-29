namespace ApiFunkosConjunta.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ApiFunkosConjunta.Data;

public static class DatabaseConfig
{
    public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        Log.Information("Configurando PostgreSQL...");
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            if (environment.IsDevelopment())
            {
                connectionString = "Host=localhost;Database=funkos;Username=admin;Password=admin123";
                Log.Warning("Usando credenciales por defecto de desarrollo");
            }
            else
            {
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection no está definida.");
            }
        }
        services.AddDbContext<FunkoDbContext>(options => options.UseNpgsql(connectionString));
        return services;
    }
}
