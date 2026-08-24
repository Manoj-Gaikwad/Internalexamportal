using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Internalexamportal.Web.ActionFilters
{
    /// <summary>
    /// Validates model state and returns bad request 
    /// with model state if validation fails
    /// </summary>
    public class ValidateModelState : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Filters.Contains(new SkipModelStateValidation()))
                return;

            if (!filterContext.ModelState.IsValid)
                filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
        }
    }
}
