using Microsoft.AspNetCore.Mvc.Filters;

namespace Internalexamportal.Web.ActionFilters
{
    /// <summary>
    /// Marker action filter to negate <see cref="ValidateModelState"/> action filter in controller
    /// </summary>
    public class SkipModelStateValidation : ActionFilterAttribute
    {
        //no code here ValidateModelState action filter checks
        //for this action filter and skips it if this is present
    }
}
