namespace ApiFunkosConjunta.Models;

using ApiFunkosConjunta.Data.Abstractions;

/// <summary>
/// Entidad de dominio que representa una categoría de Funkos.
/// Relación uno-a-muchos con Funko. Soporta soft-delete.
/// </summary>
public class Categoria : ITimestamped
{
    /// <summary>Identificador único de la categoría (PK en PostgreSQL).</summary>
    public long Id { get; set; }

    /// <summary>Nombre descriptivo de la categoría (3-100 caracteres, único).</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Descripción opcional de la categoría.</summary>
    public string? Descripcion { get; set; }

    /// <summary>Indica si la categoría está eliminada (soft-delete).</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Fecha de creación en UTC.</summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    /// <summary>Fecha de última modificación en UTC.</summary>
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

    /// <summary>Colección de Funkos asociados a esta categoría.</summary>
    public ICollection<Funko> Funkos { get; set; } = [];
}
