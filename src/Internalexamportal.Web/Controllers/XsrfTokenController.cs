using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace Internalexamportal.Web.Controllers
{
    [Route("api/[controller]")]
    public class XsrfTokenController : Controller
    {
        private readonly IAntiforgery _antiforgery;
        public XsrfTokenController(IAntiforgery antiforgery) => _antiforgery = antiforgery;

        [HttpGet]
        public IActionResult Get()
        {
            var tokens = _antiforgery.GetAndStoreTokens(HttpContext);
            return Ok(new
            {
                token = tokens.RequestToken,
                tokenName = tokens.HeaderName
            });
        }

    }
}
