namespace ApiFunkosConjunta.Models;

using ApiFunkosConjunta.Data.Abstractions;

/// <summary>
/// Entidad de dominio que representa un Funko (figura de vinilo coleccionable).
/// </summary>
public class Funko : ITimestamped
{
    /// <summary>Identificador único del Funko (PK en PostgreSQL).</summary>
    public long Id { get; set; }

    /// <summary>Nombre del Funko (3-200 caracteres).</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Precio unitario en EUR.</summary>
    public decimal Precio { get; set; }

    /// <summary>Stock disponible en almacén.</summary>
    public int Stock { get; set; }

    /// <summary>Identificador de la categoría asociada (FK).</summary>
    public long CategoriaId { get; set; }

    /// <summary>Categoría asociada (relación muchos-a-uno).</summary>
    public Categoria Categoria { get; set; } = null!;

    /// <summary>Indica si el Funko está eliminado (soft-delete).</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Fecha de creación en UTC.</summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    /// <summary>Fecha de última modificación en UTC.</summary>
    public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
}
