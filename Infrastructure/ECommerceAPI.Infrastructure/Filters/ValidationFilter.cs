using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
            {
                //if there is an error(s)
                var errors = context.ModelState.Where(x => x.Value.Errors.Any())
                    //.key brings the property, .value brings the properties errors
                    .ToDictionary(property => property.Key, property => property.Value.Errors.Select(property => property.ErrorMessage)).ToArray();

                context.Result = new BadRequestObjectResult(errors);
                return;
          }
            //for the next filter
            await next();
        }
    }
}
