namespace ApiFunkosConjunta.Controllers;

using Microsoft.AspNetCore.Mvc;
using ApiFunkosConjunta.Dtos;
using ApiFunkosConjunta.Extensions;
using ApiFunkosConjunta.Services;

/// <summary>
/// Controlador de API para gestión de Funkos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FunkosController(
    IFunkoService service,
    ILogger<FunkosController> logger
) : ControllerBase
{
    /// <summary>
    /// Obtiene todos los Funkos.
    /// </summary>
    /// <returns>200 OK con la lista de Funkos.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FunkoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FunkoDto>>> GetAll()
    {
        logger.LogInformation("GET /api/funkos");
        var result = await service.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result.Value);
        return result.Error.ToHttpResult<IEnumerable<FunkoDto>>();
    }

    /// <summary>
    /// Obtiene un Funko por su ID.
    /// </summary>
    /// <param name="id">ID del Funko.</param>
    /// <returns>200 OK con el Funko, o 404 si no existe.</returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(FunkoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FunkoDto>> GetById(long id)
    {
        logger.LogInformation("GET /api/funkos/{Id}", id);
        var result = await service.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result.Value);
        return result.Error.ToHttpResult<FunkoDto>();
    }

    /// <summary>
    /// Crea un nuevo Funko.
    /// </summary>
    /// <param name="dto">Datos del Funko a crear.</param>
    /// <returns>201 Created con el Funko creado, o 400/409 si hay errores.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(FunkoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FunkoDto>> Create([FromBody] CreateFunkoDto dto)
    {
        logger.LogInformation("POST /api/funkos - {Nombre}", dto.Nombre);
        var result = await service.CreateAsync(dto);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        return result.Error.ToHttpResult<FunkoDto>();
    }

    /// <summary>
    /// Actualiza un Funko existente.
    /// </summary>
    /// <param name="id">ID del Funko.</param>
    /// <param name="dto">Nuevos datos del Funko.</param>
    /// <returns>200 OK con el Funko actualizado, o 400/404/409 si hay errores.</returns>
    [HttpPut("{id:long}")]
    [ProducesResponseType(typeof(FunkoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<FunkoDto>> Update(long id, [FromBody] UpdateFunkoDto dto)
    {
        logger.LogInformation("PUT /api/funkos/{Id}", id);
        var result = await service.UpdateAsync(id, dto);
        if (result.IsSuccess)
            return Ok(result.Value);
        return result.Error.ToHttpResult<FunkoDto>();
    }

    /// <summary>
    /// Elimina un Funko (soft-delete).
    /// </summary>
    /// <param name="id">ID del Funko a eliminar.</param>
    /// <returns>204 No Content, o 404 si no existe.</returns>
    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        logger.LogInformation("DELETE /api/funkos/{Id}", id);
        var result = await service.DeleteAsync(id);
        if (result.IsSuccess)
            return NoContent();
        return result.Error.ToHttpResult();
    }

    /// <summary>
    /// Busca Funkos por nombre.
    /// </summary>
    /// <param name="name">Texto a buscar en el nombre.</param>
    /// <returns>200 OK con los Funkos que coinciden.</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<FunkoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FunkoDto>>> Search([FromQuery] string name)
    {
        logger.LogInformation("GET /api/funkos/search?name={Name}", name);
        var result = await service.SearchByNameAsync(name);
        if (result.IsSuccess)
            return Ok(result.Value);
        return result.Error.ToHttpResult<IEnumerable<FunkoDto>>();
    }
}
