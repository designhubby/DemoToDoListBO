using DemoToListBE.ExceptionHandle.CustomException;
using DemoToListBE.ExceptionHandle.CustomProblemDetails;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;

namespace DemoToListBE.ExceptionHandle.Map
{
    public static class ProblemDetailsMap
    {
        public static void ProlemDetailsMapConfiguration(ProblemDetailsOptions options, IHostEnvironment environement)
        {
            options.IncludeExceptionDetails = (ctx, env) => environement.IsDevelopment() || environement.IsStaging();
            options.Map<ToDoListCustomException>(exception => new ToDoListCustomDetails
            {
                Title = exception.Title,
                Detail = exception.Detail, 
                Status = StatusCodes.Status500InternalServerError,
                Type = exception.Type,
                Instance = exception.Instance,
                AdditionalInfo = exception.AdditionalInfo,
            });
            options.Map<AuthenticationException>(exception => new AuthenticationError
            {
                Title = exception.Title,
                Detail = exception.Detail,
                Status = StatusCodes.Status500InternalServerError,
                Type = exception.Type,
                Instance = exception.Instance,
            });
            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            options.MapToStatusCode<BadHttpRequestException>(StatusCodes.Status400BadRequest);
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
            options.MapToStatusCode<NotSupportedException>(StatusCodes.Status500InternalServerError);
            options.MapToStatusCode<DbUpdateException>(StatusCodes.Status500InternalServerError);
            options.MapToStatusCode<InvalidOperationException>(StatusCodes.Status500InternalServerError);
        }
    }
}
