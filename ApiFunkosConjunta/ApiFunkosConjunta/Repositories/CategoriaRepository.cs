namespace ApiFunkosConjunta.Repositories;

using Microsoft.EntityFrameworkCore;
using ApiFunkosConjunta.Data;
using ApiFunkosConjunta.Models;

/// <summary>
/// Implementación del repositorio de Categorías sobre PostgreSQL.
/// </summary>
public class CategoriaRepository(
    FunkoDbContext context,
    ILogger<CategoriaRepository> logger
) : ICategoriaRepository
{
    public async Task<IEnumerable<Categoria>> FindAllAsync()
    {
        logger.LogDebug("Buscando todas las categorías");
        return await context.Categorias.OrderBy(c => c.Nombre).AsNoTracking().ToListAsync();
    }

    public async Task<Categoria?> FindByIdAsync(long id)
    {
        logger.LogDebug("Buscando categoría por ID: {Id}", id);
        return await context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsByNombreAsync(string nombre, long? excludeId = null)
    {
        var query = context.Categorias.Where(c => c.Nombre == nombre);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<Categoria> CreateAsync(Categoria categoria)
    {
        logger.LogDebug("Creando categoría: {Nombre}", categoria.Nombre);
        context.Categorias.Add(categoria);
        await context.SaveChangesAsync();
        return categoria;
    }

    public async Task<Categoria> UpdateAsync(Categoria categoria)
    {
        logger.LogDebug("Actualizando categoría ID: {Id}", categoria.Id);
        context.Categorias.Update(categoria);
        await context.SaveChangesAsync();
        return categoria;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        logger.LogDebug("Eliminando (soft) categoría ID: {Id}", id);
        var categoria = await context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
        if (categoria is null) return false;
        categoria.IsDeleted = true;
        await context.SaveChangesAsync();
        return true;
    }
}
