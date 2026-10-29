namespace ApiFunkosConjunta.Data;

using Microsoft.EntityFrameworkCore;
using ApiFunkosConjunta.Data.Interceptors;
using ApiFunkosConjunta.Models;

/// <summary>
/// DbContext de Entity Framework Core para PostgreSQL.
/// Gestiona Funkos y Categorías.
/// </summary>
public class FunkoDbContext : DbContext
{
    private static readonly TimestampInterceptor TimestampInterceptor = new();

    /// <summary>
    /// Constructor con opciones de configuración.
    /// </summary>
    public FunkoDbContext(DbContextOptions<FunkoDbContext> options) : base(options)
    {
    }

    /// <summary>DbSet de Categorías.</summary>
    public DbSet<Categoria> Categorias { get; set; } = null!;

    /// <summary>DbSet de Funkos.</summary>
    public DbSet<Funko> Funkos { get; set; } = null!;

    /// <summary>
    /// Configura el contexto (interceptores de marcas de tiempo).
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.AddInterceptors(TimestampInterceptor);
    }

    /// <summary>
    /// Define el modelo del esquema: entidades, relaciones, índices y filtros globales.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("categorias");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).UseIdentityAlwaysColumn();
            entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Descripcion).HasMaxLength(500);
            entity.HasIndex(c => c.Nombre).IsUnique();
            entity.Property(c => c.IsDeleted).HasDefaultValue(false);
            entity.ConfigureTimestamps();
            entity.HasQueryFilter(c => !c.IsDeleted);
        });

        modelBuilder.Entity<Funko>(entity =>
        {
            entity.ToTable("funkos");
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id).UseIdentityAlwaysColumn();
            entity.Property(f => f.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(f => f.Precio).HasPrecision(10, 2);
            entity.Property(f => f.Stock).IsRequired();
            entity.Property(f => f.IsDeleted).HasDefaultValue(false);
            entity.ConfigureTimestamps();
            entity.HasOne(f => f.Categoria)
                .WithMany(c => c.Funkos)
                .HasForeignKey(f => f.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(f => f.CategoriaId);
            entity.HasIndex(f => f.Nombre);
            entity.HasQueryFilter(f => !f.IsDeleted);
        });
    }
}
