using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Antibody.CareToKnowPro.CRM.Filters
{
    /// <summary>
    /// Filter which converts a FluentValidation exception (which happened
    /// in the Mediatr validation pipeline stage) to an HTTP 400 Bad Request.
    /// </summary>
    public class FluentValidationActionFilter : IActionFilter
    {
        private readonly ApiBehaviorOptions _apiOptions;

        public FluentValidationActionFilter(IOptions<ApiBehaviorOptions> apiOptions)
        {
            _apiOptions = apiOptions.Value;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                var validationException = filterContext.Exception as ValidationException;

                // convert the fluentvalidation validationexception into an HTTP 400 response
                if (validationException != null && validationException.Errors.Count() > 0)
                {
                    foreach (var error in validationException.Errors)
                    {
                        filterContext.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }

                    filterContext.Result = _apiOptions.InvalidModelStateResponseFactory(filterContext);
                    filterContext.Exception = null;
                    filterContext.ExceptionHandled = true;
                }
            }
        }
    }
}
