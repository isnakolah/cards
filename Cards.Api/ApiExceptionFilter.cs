using Cards.Application.Common.Behaviours;
using Cards.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cards.Api;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException exception)
        {
            var details = ApiResponse<object>.Error("One or more validation errors occurred, check errors for more details",exception.Errors.Values.SelectMany(x => x).ToArray());

            context.Result = new BadRequestObjectResult(details);
        }
        else if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage)).ToArray();
            var details = ApiResponse<object>.Error("One or more validation errors occurred, check errors for more details", errors);

            context.Result = new BadRequestObjectResult(details);
        }
        else
        {
            var details = ApiResponse<object>.Error("An error occurred, check errors for more details", new[] { context.Exception.Message });

            context.Result = new ObjectResult(details)
            {
                StatusCode = 500
            };
        }
        context.ExceptionHandled = true;
    }
}