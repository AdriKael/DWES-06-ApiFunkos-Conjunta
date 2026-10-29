namespace ApiFunkosConjunta.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ApiFunkosConjunta.Errors;

/// <summary>
/// Centraliza el mapeo DomainError -> HTTP.
/// </summary>
public static class DomainErrorExtensions
{
    public static IActionResult ToHttpResult(this DomainError error) => error switch
    {
        NotFoundError e => new NotFoundObjectResult(new { message = e.Message }),
        ValidationError e => new BadRequestObjectResult(new { message = e.Message, errors = e.ValidationErrors }),
        ConflictError e => new ConflictObjectResult(new { message = e.Message }),
        BusinessRuleError e => new ObjectResult(new { message = e.Message }) { StatusCode = StatusCodes.Status422UnprocessableEntity },
        _ => new ObjectResult(new { message = error.Message }) { StatusCode = StatusCodes.Status500InternalServerError }
    };

    public static ActionResult<T> ToHttpResult<T>(this DomainError error) => error switch
    {
        NotFoundError e => new NotFoundObjectResult(new { message = e.Message }),
        ValidationError e => new BadRequestObjectResult(new { message = e.Message, errors = e.ValidationErrors }),
        ConflictError e => new ConflictObjectResult(new { message = e.Message }),
        BusinessRuleError e => new ObjectResult(new { message = e.Message }) { StatusCode = StatusCodes.Status422UnprocessableEntity },
        _ => new ObjectResult(new { message = error.Message }) { StatusCode = StatusCodes.Status500InternalServerError }
    };
}
