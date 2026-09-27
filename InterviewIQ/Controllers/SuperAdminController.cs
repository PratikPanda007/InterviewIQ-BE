using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterviewIQ.Controllers
{
    [ApiController]
    [Route("api/super-admin")]
    [Authorize(Roles = "Super Admin")]
    public class SuperAdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return Ok(new
            {
                message = "Super Admin dashboard accessed successfully."
            });
        }
    }
}
