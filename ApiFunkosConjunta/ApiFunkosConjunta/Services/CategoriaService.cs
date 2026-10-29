namespace ApiFunkosConjunta.Services;

using CSharpFunctionalExtensions;
using ApiFunkosConjunta.Dtos;
using ApiFunkosConjunta.Errors;
using ApiFunkosConjunta.Errors.Categorias;
using ApiFunkosConjunta.Mappers;
using ApiFunkosConjunta.Models;
using ApiFunkosConjunta.Repositories;

public interface ICategoriaService
{
    Task<Result<IEnumerable<CategoriaDto>, DomainError>> GetAllAsync();
    Task<Result<CategoriaDto, DomainError>> GetByIdAsync(long id);
    Task<Result<CategoriaDto, DomainError>> CreateAsync(CreateCategoriaDto dto);
    Task<Result<CategoriaDto, DomainError>> UpdateAsync(long id, CreateCategoriaDto dto);
    Task<Result<bool, DomainError>> DeleteAsync(long id);
}

public class CategoriaService(
    ICategoriaRepository repository,
    ILogger<CategoriaService> logger
) : ICategoriaService
{
    public async Task<Result<IEnumerable<CategoriaDto>, DomainError>> GetAllAsync()
    {
        logger.LogInformation("Obteniendo todas las categorías");
        var categorias = await repository.FindAllAsync();
        return Result.Success<IEnumerable<CategoriaDto>, DomainError>(categorias.Select(c => c.ToDto()).ToList());
    }

    public async Task<Result<CategoriaDto, DomainError>> GetByIdAsync(long id)
    {
        logger.LogInformation("Obteniendo categoría con ID: {Id}", id);
        var categoria = await repository.FindByIdAsync(id);
        if (categoria is null)
            return Result.Failure<CategoriaDto, DomainError>(CategoriaError.NotFound(id));
        return Result.Success<CategoriaDto, DomainError>(categoria.ToDto());
    }

    public async Task<Result<CategoriaDto, DomainError>> CreateAsync(CreateCategoriaDto dto)
    {
        logger.LogInformation("Creando nueva categoría: {Nombre}", dto.Nombre);
        if (await repository.ExistsByNombreAsync(dto.Nombre))
            return Result.Failure<CategoriaDto, DomainError>(CategoriaError.NombreDuplicado(dto.Nombre));
        var categoria = dto.ToEntity();
        var created = await repository.CreateAsync(categoria);
        return Result.Success<CategoriaDto, DomainError>(created.ToDto());
    }

    public async Task<Result<CategoriaDto, DomainError>> UpdateAsync(long id, CreateCategoriaDto dto)
    {
        logger.LogInformation("Actualizando categoría con ID: {Id}", id);
        var existing = await repository.FindByIdAsync(id);
        if (existing is null)
            return Result.Failure<CategoriaDto, DomainError>(CategoriaError.NotFound(id));
        if (await repository.ExistsByNombreAsync(dto.Nombre, id))
            return Result.Failure<CategoriaDto, DomainError>(CategoriaError.NombreDuplicado(dto.Nombre));
        existing.Nombre = dto.Nombre;
        existing.Descripcion = dto.Descripcion;
        var updated = await repository.UpdateAsync(existing);
        return Result.Success<CategoriaDto, DomainError>(updated.ToDto());
    }

    public async Task<Result<bool, DomainError>> DeleteAsync(long id)
    {
        logger.LogInformation("Eliminando categoría con ID: {Id}", id);
        var existing = await repository.FindByIdAsync(id);
        if (existing is null)
            return Result.Failure<bool, DomainError>(CategoriaError.NotFound(id));
        var deleted = await repository.DeleteAsync(id);
        if (!deleted)
            return Result.Failure<bool, DomainError>(CategoriaError.NotFound(id));
        return Result.Success<bool, DomainError>(true);
    }
}
