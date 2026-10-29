namespace ApiFunkosConjunta.Repositories;

using ApiFunkosConjunta.Models;

/// <summary>
/// Contrato del repositorio de Funkos.
/// </summary>
public interface IFunkoRepository
{
    Task<IEnumerable<Funko>> FindAllAsync();
    Task<Funko?> FindByIdAsync(long id);
    Task<IEnumerable<Funko>> FindByNameAsync(string name);
    Task<bool> ExistsByNombreAsync(string nombre, long? excludeId = null);
    Task<bool> CategoriaExistsAsync(long categoriaId);
    Task<Funko> CreateAsync(Funko funko);
    Task<Funko> UpdateAsync(Funko funko);
    Task<bool> DeleteAsync(long id);
}
