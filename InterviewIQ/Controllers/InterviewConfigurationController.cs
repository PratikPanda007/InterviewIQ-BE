using InterviewIQ.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewIQ.Controllers
{
    [ApiController]
    [Route("api/interview-configuration")]
    [Authorize]
    public class InterviewConfigurationController : ControllerBase
    {
        private readonly InterviewIQDbContext _context;

        public InterviewConfigurationController(
            InterviewIQDbContext context)
        {
            _context = context;
        }

        // GET: api/interview-configuration/domains
        [HttpGet("domains")]
        public async Task<IActionResult> GetDomains()
        {
            var domains = await _context.InterviewDomains
                .AsNoTracking()
                .Where(x => x.IsActive == 1)
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description
                })
                .ToListAsync();

            return Ok(domains);
        }

        // GET: api/interview-configuration/domains/1/topics
        [HttpGet("domains/{id:int}/topics")]
        public async Task<IActionResult> GetTopics(int id)
        {
            var domainExists = await _context.InterviewDomains
                .AnyAsync(x => x.Id == id && x.IsActive == 1);

            if (!domainExists)
            {
                return NotFound(new
                {
                    message = "Interview domain not found."
                });
            }

            var topics = await _context.Topics
                .AsNoTracking()
                .Where(x =>
                    x.DomainId == id &&
                    x.IsActive == 1)
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    x.Id,
                    x.DomainId,
                    x.Name,
                    x.Description
                })
                .ToListAsync();

            return Ok(topics);
        }

        // GET: api/interview-configuration/difficulties
        [HttpGet("difficulties")]
        public async Task<IActionResult> GetDifficulties()
        {
            var difficulties = await _context.InterviewDifficulties
                .AsNoTracking()
                .Where(x => x.IsActive == 1)
                .OrderBy(x => x.SortOrder)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.SortOrder
                })
                .ToListAsync();

            return Ok(difficulties);
        }

        // GET: api/interview-configuration/durations
        [HttpGet("durations")]
        public async Task<IActionResult> GetDurations()
        {
            var durations = await _context.InterviewDurations
                .AsNoTracking()
                .Where(x => x.IsActive == 1)
                .OrderBy(x => x.SortOrder)
                .Select(x => new
                {
                    x.Id,
                    x.DurationMinutes,
                    x.SortOrder
                })
                .ToListAsync();

            return Ok(durations);
        }

        // GET: api/interview-configuration/personas
        [HttpGet("personas")]
        public async Task<IActionResult> GetPersonas()
        {
            var personas = await _context.InterviewerPersonas
                .AsNoTracking()
                .Where(x => x.IsActive == 1)
                .OrderBy(x => x.SortOrder)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.SortOrder
                })
                .ToListAsync();

            return Ok(personas);
        }
    }
}
