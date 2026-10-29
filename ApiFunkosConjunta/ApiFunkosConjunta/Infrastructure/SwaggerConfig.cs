namespace ApiFunkosConjunta.Infrastructure;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Serilog;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        Log.Information("Configurando Swagger/OpenAPI...");
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ApiFunkosConjunta - API REST de Funkos",
                Version = "v1",
                Description = "API REST de gestión de Funkos (CRUD) con PostgreSQL y Redis.",
                Contact = new OpenApiContact { Name = "DWES" }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        });
        return services;
    }
}
