using Microsoft.EntityFrameworkCore;
using Serilog;
using ApiFunkosConjunta.Data;
using ApiFunkosConjunta.Infrastructure;
using ApiFunkosConjunta.Middleware;
using ApiFunkosConjunta.Models;

// Configuración de Serilog: salida a consola y fichero con log estructurado.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/funkos-.log", rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDatabases(builder.Configuration, builder.Environment);
builder.Services.AddCache(builder.Environment, builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandler();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiFunkosConjunta v1");
        options.RoutePrefix = "swagger";
    });
}
else
{
    // En producción también exponemos Swagger para documentación.
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiFunkosConjunta v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.MapControllers();

// Health check simple.
app.MapGet("/health", () => Results.Ok(new { status = "OK", timestamp = DateTime.UtcNow }));

// Inicialización de base de datos y datos semilla.
await InitializeDatabaseAsync(app);

app.Run();

static async Task InitializeDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<FunkoDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    logger.LogInformation("Inicializando base de datos...");
    await context.Database.EnsureCreatedAsync();

    // Semilla: categoría por defecto si no existe ninguna.
    if (!await context.Categorias.AnyAsync())
    {
        context.Categorias.Add(new Categoria { Nombre = "General", Descripcion = "Categoría por defecto" });
        await context.SaveChangesAsync();
        logger.LogInformation("Categoría por defecto creada.");
    }
}

// Exponer Program para WebApplicationFactory en tests de integraci�n.
public partial class Program { }
