namespace ApiFunkosConjunta.Data.Abstractions;

/// <summary>
/// Interfaz de auditoría para entidades con marcas de tiempo.
/// </summary>
public interface ITimestamped
{
    /// <summary>Fecha de creación en UTC (solo lectura).</summary>
    DateTime CreatedAt { get; }

    /// <summary>Fecha de última modificación en UTC.</summary>
    DateTime UpdatedAt { get; }
}
