namespace ApiFunkosConjunta.Dtos;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO de salida para un Funko.
/// </summary>
public class FunkoDto
{
    /// <summary>Identificador único.</summary>
    public long Id { get; set; }

    /// <summary>Nombre del Funko.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Precio unitario.</summary>
    public decimal Precio { get; set; }

    /// <summary>Stock disponible.</summary>
    public int Stock { get; set; }

    /// <summary>Identificador de la categoría.</summary>
    public long CategoriaId { get; set; }

    /// <summary>Nombre de la categoría.</summary>
    public string CategoriaNombre { get; set; } = string.Empty;

    /// <summary>Indica si está eliminado (soft-delete).</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Fecha de creación.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Fecha de última modificación.</summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// DTO de entrada para crear un Funko.
/// </summary>
public class CreateFunkoDto
{
    /// <summary>Nombre del Funko (3-200 caracteres).</summary>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Precio unitario (mayor que 0).</summary>
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal Precio { get; set; }

    /// <summary>Stock disponible (no negativo).</summary>
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    /// <summary>Identificador de la categoría a la que pertenece.</summary>
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [Range(1, long.MaxValue, ErrorMessage = "Debe indicar una categoría válida.")]
    public long CategoriaId { get; set; }
}

/// <summary>
/// DTO de entrada para actualizar un Funko completo.
/// </summary>
public class UpdateFunkoDto
{
    /// <summary>Nuevo nombre del Funko (3-200 caracteres).</summary>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 200 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Nuevo precio unitario (mayor que 0).</summary>
    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor que 0.")]
    public decimal Precio { get; set; }

    /// <summary>Nuevo stock disponible (no negativo).</summary>
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
    public int Stock { get; set; }

    /// <summary>Nueva categoría.</summary>
    [Required(ErrorMessage = "La categoría es obligatoria.")]
    [Range(1, long.MaxValue, ErrorMessage = "Debe indicar una categoría válida.")]
    public long CategoriaId { get; set; }
}
