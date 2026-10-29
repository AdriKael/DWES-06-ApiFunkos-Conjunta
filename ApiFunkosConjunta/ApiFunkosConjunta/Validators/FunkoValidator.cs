namespace ApiFunkosConjunta.Validators;

using ApiFunkosConjunta.Dtos;
using ApiFunkosConjunta.Errors;
using ApiFunkosConjunta.Errors.Categorias;
using ApiFunkosConjunta.Errors.Funkos;
using ApiFunkosConjunta.Repositories;

/// <summary>
/// Validador de dominio para Funkos.
/// </summary>
public interface IFunkoValidator
{
    /// <summary>Valida la creación de un Funko.</summary>
    Task<DomainError?> ValidateCreateAsync(CreateFunkoDto dto);

    /// <summary>Valida la actualización de un Funko.</summary>
    Task<DomainError?> ValidateUpdateAsync(long id, UpdateFunkoDto dto);
}

/// <summary>
/// Implementación del validador de dominio para Funkos.
/// </summary>
public class FunkoValidator(
    IFunkoRepository repository
) : IFunkoValidator
{
    public async Task<DomainError?> ValidateCreateAsync(CreateFunkoDto dto)
    {
        if (await repository.ExistsByNombreAsync(dto.Nombre))
            return FunkoError.NombreDuplicado(dto.Nombre);

        if (!await repository.CategoriaExistsAsync(dto.CategoriaId))
            return FunkoError.CategoriaNoEncontrada(dto.CategoriaId);

        return null;
    }

    public async Task<DomainError?> ValidateUpdateAsync(long id, UpdateFunkoDto dto)
    {
        if (await repository.ExistsByNombreAsync(dto.Nombre, id))
            return FunkoError.NombreDuplicado(dto.Nombre);

        if (!await repository.CategoriaExistsAsync(dto.CategoriaId))
            return FunkoError.CategoriaNoEncontrada(dto.CategoriaId);

        return null;
    }
}
