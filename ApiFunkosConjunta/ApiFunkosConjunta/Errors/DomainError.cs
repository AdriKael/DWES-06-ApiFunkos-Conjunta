namespace ApiFunkosConjunta.Errors;

/// <summary>
/// Clase base para errores del dominio.
/// </summary>
public abstract record DomainError(string Message)
{
    /// <inheritdoc/>
    public override string ToString() => $"{GetType().Name}: {Message}";
}

/// <summary>Recurso no encontrado (HTTP 404).</summary>
public sealed record NotFoundError(string Message) : DomainError(Message)
{
    public static NotFoundError FromId(long id, string resourceType = "Unknown") =>
        new($"Recurso con ID {id} no encontrado");
}

/// <summary>Error de validación de datos (HTTP 400).</summary>
public sealed record ValidationError(string Message, Dictionary<string, string[]> ValidationErrors)
    : DomainError(Message)
{
    public static ValidationError WithFieldErrors(Dictionary<string, string[]> fieldErrors) =>
        new("Errores de validación", fieldErrors);

    public static ValidationError Create(string message) =>
        new(message, new Dictionary<string, string[]>());
}

/// <summary>Violación de regla de negocio (HTTP 422).</summary>
public sealed record BusinessRuleError(string Message) : DomainError(Message);

/// <summary>Conflicto de recursos (HTTP 409).</summary>
public sealed record ConflictError(string Message) : DomainError(Message)
{
    public static ConflictError Duplicate(string resourceType, string value) =>
        new($"Ya existe un {resourceType} con el valor '{value}'");
}
