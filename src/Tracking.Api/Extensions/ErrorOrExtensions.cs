using ErrorOr;

namespace Tracking.Api.Extensions;

public static class ErrorOrExtensions
{
    public static IResult ToProblem(this List<Error> errors)
    {
        if (errors.Count == 0) return Results.Problem();
        if (errors.All(e => e.Type == ErrorType.Validation))
        {
            var failures = errors.ToDictionary(
                e => e.Code,
                e => new[] { e.Description });

            return Results.ValidationProblem(failures);
        }

        var first = errors[0];
        var statusCode = first.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(
            title: first.Code,
            detail: first.Description,
            statusCode: statusCode);
    }
}