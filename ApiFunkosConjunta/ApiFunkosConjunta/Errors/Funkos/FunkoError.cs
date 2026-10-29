namespace ApiFunkosConjunta.Errors.Funkos;

using ApiFunkosConjunta.Errors;

/// <summary>
/// Errores del dominio de Funkos.
/// </summary>
public static class FunkoError
{
    public static NotFoundError NotFound(long id) =>
        NotFoundError.FromId(id, "Funko");

    public static NotFoundError CategoriaNoEncontrada(long categoriaId) =>
        NotFoundError.FromId(categoriaId, "Categoria");

    public static ConflictError NombreDuplicado(string nombre) =>
        ConflictError.Duplicate("Funko", nombre);

    public static BusinessRuleError StockInsuficiente(string nombre, int disponible, int solicitado) =>
        new($"Stock insuficiente para el Funko '{nombre}'. Disponible: {disponible}, Solicitado: {solicitado}");

    public static ValidationError Validacion(string mensaje) =>
        ValidationError.Create(mensaje);
}
