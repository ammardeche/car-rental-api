using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Api.Controllers
{
    [ApiController]
    [Route("api/debug")]
    public class DebugController : ControllerBase
    {
        [HttpGet("headers")]
        [AllowAnonymous]
        public IActionResult Headers()
        {
            var headers = Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
            var auth = Request.Headers.ContainsKey("Authorization") ? Request.Headers["Authorization"].ToString() : null;
            var authPreview = auth == null ? null : (auth.Length > 20 ? auth.Substring(0, 20) + "..." : auth);
            return Ok(new { hasAuthorization = auth != null, authorizationPreview = authPreview, headers });
        }
    }
}