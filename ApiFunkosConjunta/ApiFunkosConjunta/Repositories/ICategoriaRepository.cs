namespace ApiFunkosConjunta.Repositories;

using ApiFunkosConjunta.Models;

/// <summary>
/// Contrato del repositorio de Categorías.
/// </summary>
public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> FindAllAsync();
    Task<Categoria?> FindByIdAsync(long id);
    Task<bool> ExistsByNombreAsync(string nombre, long? excludeId = null);
    Task<Categoria> CreateAsync(Categoria categoria);
    Task<Categoria> UpdateAsync(Categoria categoria);
    Task<bool> DeleteAsync(long id);
}
