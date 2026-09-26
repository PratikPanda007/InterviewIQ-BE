using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InterviewIQ.Models;

namespace InterviewIQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: api/home
        [HttpGet] 
        public IActionResult Get() 
        { 
            return Ok(new { message = "Hello from HomeController (GET)", serverTime = DateTime.UtcNow }); 
        }

        // POST: api/home
        [HttpPost]
        public IActionResult Post([FromBody] HomeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(new
            {
                message = "Data received successfully (POST)",
                receivedData = request
            });
        }
    }
}
