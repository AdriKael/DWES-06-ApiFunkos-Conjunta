namespace ApiFunkosConjunta.Mappers;

using ApiFunkosConjunta.Dtos;
using ApiFunkosConjunta.Models;

/// <summary>
/// Métodos de extensión para mapear entre entidades y DTOs.
/// </summary>
public static class FunkoMappers
{
    /// <summary>Convierte una entidad Funko a FunkoDto.</summary>
    public static FunkoDto ToDto(this Funko funko) => new()
    {
        Id = funko.Id,
        Nombre = funko.Nombre,
        Precio = funko.Precio,
        Stock = funko.Stock,
        CategoriaId = funko.CategoriaId,
        CategoriaNombre = funko.Categoria?.Nombre ?? string.Empty,
        IsDeleted = funko.IsDeleted,
        CreatedAt = funko.CreatedAt,
        UpdatedAt = funko.UpdatedAt
    };

    /// <summary>Convierte un CreateFunkoDto a entidad Funko.</summary>
    public static Funko ToEntity(this CreateFunkoDto dto) => new()
    {
        Nombre = dto.Nombre,
        Precio = dto.Precio,
        Stock = dto.Stock,
        CategoriaId = dto.CategoriaId
    };

    /// <summary>Aplica los valores de UpdateFunkoDto sobre una entidad existente.</summary>
    public static void UpdateFromDto(this Funko funko, UpdateFunkoDto dto)
    {
        funko.Nombre = dto.Nombre;
        funko.Precio = dto.Precio;
        funko.Stock = dto.Stock;
        funko.CategoriaId = dto.CategoriaId;
    }
}

public static class CategoriaMappers
{
    public static CategoriaDto ToDto(this Categoria categoria) => new()
    {
        Id = categoria.Id,
        Nombre = categoria.Nombre,
        Descripcion = categoria.Descripcion,
        IsDeleted = categoria.IsDeleted,
        CreatedAt = categoria.CreatedAt,
        UpdatedAt = categoria.UpdatedAt
    };

    public static Categoria ToEntity(this CreateCategoriaDto dto) => new()
    {
        Nombre = dto.Nombre,
        Descripcion = dto.Descripcion
    };
}
