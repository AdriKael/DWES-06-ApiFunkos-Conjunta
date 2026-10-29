namespace ApiFunkosConjunta.Errors.Categorias;

using ApiFunkosConjunta.Errors;

/// <summary>
/// Errores del dominio de Categorías.
/// </summary>
public static class CategoriaError
{
    public static NotFoundError NotFound(long id) =>
        NotFoundError.FromId(id, "Categoria");

    public static ConflictError NombreDuplicado(string nombre) =>
        ConflictError.Duplicate("Categoria", nombre);

    public static ValidationError Validacion(string mensaje) =>
        ValidationError.Create(mensaje);
}
