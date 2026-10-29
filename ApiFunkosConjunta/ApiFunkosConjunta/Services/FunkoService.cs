namespace ApiFunkosConjunta.Services;

using CSharpFunctionalExtensions;
using ApiFunkosConjunta.Dtos;
using ApiFunkosConjunta.Errors;
using ApiFunkosConjunta.Mappers;
using ApiFunkosConjunta.Models;
using ApiFunkosConjunta.Errors.Funkos;
using ApiFunkosConjunta.Repositories;
using ApiFunkosConjunta.Services.Cache;
using ApiFunkosConjunta.Validators;

/// <summary>
/// Contrato del servicio de Funkos.
/// </summary>
public interface IFunkoService
{
    Task<Result<IEnumerable<FunkoDto>, DomainError>> GetAllAsync();
    Task<Result<FunkoDto, DomainError>> GetByIdAsync(long id);
    Task<Result<FunkoDto, DomainError>> CreateAsync(CreateFunkoDto dto);
    Task<Result<FunkoDto, DomainError>> UpdateAsync(long id, UpdateFunkoDto dto);
    Task<Result<bool, DomainError>> DeleteAsync(long id);
    Task<Result<IEnumerable<FunkoDto>, DomainError>> SearchByNameAsync(string name);
}

/// <summary>
/// Implementación del servicio de Funkos con caché y patrón Result.
/// </summary>
public class FunkoService(
    IFunkoRepository repository,
    IFunkoValidator validator,
    ICacheService cache,
    ILogger<FunkoService> logger
) : IFunkoService
{
    private const string CacheKeyAll = "funkos:all";
    private static string CacheKeyById(long id) => $"funkos:{id}";

    public async Task<Result<IEnumerable<FunkoDto>, DomainError>> GetAllAsync()
    {
        logger.LogInformation("Obteniendo todos los Funkos");

        var cached = await cache.GetAsync<IEnumerable<FunkoDto>>(CacheKeyAll);
        if (cached is not null)
            return Result.Success<IEnumerable<FunkoDto>, DomainError>(cached);

        var funkos = await repository.FindAllAsync();
        var dtos = funkos.Select(f => f.ToDto()).ToList();
        await cache.SetAsync(CacheKeyAll, dtos);
        return Result.Success<IEnumerable<FunkoDto>, DomainError>(dtos);
    }

    public async Task<Result<FunkoDto, DomainError>> GetByIdAsync(long id)
    {
        logger.LogInformation("Obteniendo Funko con ID: {Id}", id);

        var cached = await cache.GetAsync<FunkoDto>(CacheKeyById(id));
        if (cached is not null)
            return Result.Success<FunkoDto, DomainError>(cached);

        var funko = await repository.FindByIdAsync(id);
        if (funko is null)
            return Result.Failure<FunkoDto, DomainError>(FunkoError.NotFound(id));

        var dto = funko.ToDto();
        await cache.SetAsync(CacheKeyById(id), dto);
        return Result.Success<FunkoDto, DomainError>(dto);
    }

    public async Task<Result<FunkoDto, DomainError>> CreateAsync(CreateFunkoDto dto)
    {
        logger.LogInformation("Creando nuevo Funko: {Nombre}", dto.Nombre);

        var validationError = await validator.ValidateCreateAsync(dto);
        if (validationError is not null)
            return Result.Failure<FunkoDto, DomainError>(validationError);

        var funko = dto.ToEntity();
        var created = await repository.CreateAsync(funko);
        var resultDto = created.ToDto();

        await cache.RemoveAsync(CacheKeyAll);
        return Result.Success<FunkoDto, DomainError>(resultDto);
    }

    public async Task<Result<FunkoDto, DomainError>> UpdateAsync(long id, UpdateFunkoDto dto)
    {
        logger.LogInformation("Actualizando Funko con ID: {Id}", id);

        var existing = await repository.FindByIdAsync(id);
        if (existing is null)
            return Result.Failure<FunkoDto, DomainError>(FunkoError.NotFound(id));

        var validationError = await validator.ValidateUpdateAsync(id, dto);
        if (validationError is not null)
            return Result.Failure<FunkoDto, DomainError>(validationError);

        existing.UpdateFromDto(dto);
        var updated = await repository.UpdateAsync(existing);
        var resultDto = updated.ToDto();

        await cache.RemoveAsync(CacheKeyAll);
        await cache.RemoveAsync(CacheKeyById(id));
        return Result.Success<FunkoDto, DomainError>(resultDto);
    }

    public async Task<Result<bool, DomainError>> DeleteAsync(long id)
    {
        logger.LogInformation("Eliminando Funko con ID: {Id}", id);

        var existing = await repository.FindByIdAsync(id);
        if (existing is null)
            return Result.Failure<bool, DomainError>(FunkoError.NotFound(id));

        var deleted = await repository.DeleteAsync(id);
        if (!deleted)
            return Result.Failure<bool, DomainError>(FunkoError.NotFound(id));

        await cache.RemoveAsync(CacheKeyAll);
        await cache.RemoveAsync(CacheKeyById(id));
        return Result.Success<bool, DomainError>(true);
    }

    public async Task<Result<IEnumerable<FunkoDto>, DomainError>> SearchByNameAsync(string name)
    {
        logger.LogInformation("Buscando Funkos por nombre: {Name}", name);

        var funkos = await repository.FindByNameAsync(name);
        var dtos = funkos.Select(f => f.ToDto()).ToList();
        return Result.Success<IEnumerable<FunkoDto>, DomainError>(dtos);
    }
}
