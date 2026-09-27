using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InterviewIQ.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin,Super Admin")]
    public class AdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return Ok(new
            {
                message = "Admin dashboard accessed successfully."
            });
        }
    }
}
