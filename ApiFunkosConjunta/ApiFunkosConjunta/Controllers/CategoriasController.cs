namespace ApiFunkosConjunta.Controllers;

using Microsoft.AspNetCore.Mvc;
using ApiFunkosConjunta.Dtos;
using ApiFunkosConjunta.Extensions;
using ApiFunkosConjunta.Services;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriasController(
    ICategoriaService service,
    ILogger<CategoriasController> logger
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
    {
        logger.LogInformation("GET /api/categorias");
        var result = await service.GetAllAsync();
        if (result.IsSuccess) return Ok(result.Value);
        return result.Error.ToHttpResult<IEnumerable<CategoriaDto>>();
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<CategoriaDto>> GetById(long id)
    {
        logger.LogInformation("GET /api/categorias/{Id}", id);
        var result = await service.GetByIdAsync(id);
        if (result.IsSuccess) return Ok(result.Value);
        return result.Error.ToHttpResult<CategoriaDto>();
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Create([FromBody] CreateCategoriaDto dto)
    {
        logger.LogInformation("POST /api/categorias - {Nombre}", dto.Nombre);
        var result = await service.CreateAsync(dto);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        return result.Error.ToHttpResult<CategoriaDto>();
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<CategoriaDto>> Update(long id, [FromBody] CreateCategoriaDto dto)
    {
        logger.LogInformation("PUT /api/categorias/{Id}", id);
        var result = await service.UpdateAsync(id, dto);
        if (result.IsSuccess) return Ok(result.Value);
        return result.Error.ToHttpResult<CategoriaDto>();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        logger.LogInformation("DELETE /api/categorias/{Id}", id);
        var result = await service.DeleteAsync(id);
        if (result.IsSuccess) return NoContent();
        return result.Error.ToHttpResult();
    }
}
