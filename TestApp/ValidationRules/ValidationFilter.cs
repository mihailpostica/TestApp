using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.ValidationRules
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                
                var errorResponse = new ErrorResponse();
                var errors =context.ModelState.Where(x=>x.Value.Errors.Count>0).ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage)).ToArray();
                foreach (var error in errors)
                {
                    var validationError = new ValidationError();
                    validationError.Field = error.Key;
                    foreach (var subError in error.Value)
                    {
                        validationError.Errors.Add(subError);
                    }
                    errorResponse.Errors.Add(validationError);
                }
                context.Result = new BadRequestObjectResult(errorResponse);
                return;
            }
            await next();
        }
    }
}