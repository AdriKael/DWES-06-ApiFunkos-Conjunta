namespace ApiFunkosConjunta.Repositories;

using Microsoft.EntityFrameworkCore;
using ApiFunkosConjunta.Data;
using ApiFunkosConjunta.Models;

/// <summary>
/// Implementación del repositorio de Funkos sobre PostgreSQL.
/// </summary>
public class FunkoRepository(
    FunkoDbContext context,
    ILogger<FunkoRepository> logger
) : IFunkoRepository
{
    public async Task<IEnumerable<Funko>> FindAllAsync()
    {
        logger.LogDebug("Buscando todos los Funkos");
        return await context.Funkos
            .Include(f => f.Categoria)
            .OrderBy(f => f.Nombre)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Funko?> FindByIdAsync(long id)
    {
        logger.LogDebug("Buscando Funko por ID: {Id}", id);
        return await context.Funkos
            .Include(f => f.Categoria)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<Funko>> FindByNameAsync(string name)
    {
        logger.LogDebug("Buscando Funkos por nombre: {Name}", name);
        return await context.Funkos
            .Include(f => f.Categoria)
            .Where(f => f.Nombre.Contains(name))
            .OrderBy(f => f.Nombre)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> ExistsByNombreAsync(string nombre, long? excludeId = null)
    {
        var query = context.Funkos.Where(f => f.Nombre == nombre);
        if (excludeId.HasValue)
            query = query.Where(f => f.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<bool> CategoriaExistsAsync(long categoriaId)
    {
        return await context.Categorias.AnyAsync(c => c.Id == categoriaId);
    }

    public async Task<Funko> CreateAsync(Funko funko)
    {
        logger.LogDebug("Creando Funko: {Nombre}", funko.Nombre);
        context.Funkos.Add(funko);
        await context.SaveChangesAsync();
        // recargar categoría
        await context.Entry(funko).Reference(f => f.Categoria).LoadAsync();
        return funko;
    }

    public async Task<Funko> UpdateAsync(Funko funko)
    {
        logger.LogDebug("Actualizando Funko ID: {Id}", funko.Id);
        context.Funkos.Update(funko);
        await context.SaveChangesAsync();
        await context.Entry(funko).Reference(f => f.Categoria).LoadAsync();
        return funko;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        logger.LogDebug("Eliminando (soft) Funko ID: {Id}", id);
        var funko = await context.Funkos.FirstOrDefaultAsync(f => f.Id == id);
        if (funko is null) return false;
        funko.IsDeleted = true;
        await context.SaveChangesAsync();
        return true;
    }
}
