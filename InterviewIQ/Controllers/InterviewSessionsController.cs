using InterviewIQ.Data;
using InterviewIQ.Models.Interviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace InterviewIQ.Controllers
{
    [Route("api/interview-sessions")]
    [Authorize]
    public class InterviewSessionsController : ControllerBase
    {
        private readonly InterviewIQDbContext _context;

        public InterviewSessionsController(InterviewIQDbContext context)
        {
            _context = context;
        }

        // POST: api/interview-sessions
        [HttpPost]
        public async Task<IActionResult> CreateSession(
            [FromBody] CreateInterviewSessionRequest request)
        {
            // Get the user ID from the authenticated JWT.
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var userExists = await _context.Users.AnyAsync(x =>
                x.Id == userId && x.IsActive == 1);

            if (!userExists)
            {
                return Unauthorized(new
                {
                    message = "User account is unavailable."
                });
            }

            // Validate the selected domain.
            var domainExists = await _context.InterviewDomains.AnyAsync(x =>
                x.Id == request.DomainId && x.IsActive == 1);

            if (!domainExists)
            {
                return BadRequest(new
                {
                    message = "Invalid interview domain."
                });
            }

            // Require at least one topic and reject duplicate topic IDs.
            if (request.TopicIds == null ||
                request.TopicIds.Count == 0 ||
                request.TopicIds.Any(id => id <= 0) ||
                request.TopicIds.Distinct().Count() != request.TopicIds.Count)
            {
                return BadRequest(new
                {
                    message = "Select at least one valid, unique topic."
                });
            }

            // Every selected topic must be active and belong to the chosen domain.
            var validTopicIds = await _context.Topics
                .Where(x =>
                    request.TopicIds.Contains(x.Id) &&
                    x.DomainId == request.DomainId &&
                    x.IsActive == 1)
                .Select(x => x.Id)
                .ToListAsync();

            if (validTopicIds.Count != request.TopicIds.Count)
            {
                return BadRequest(new
                {
                    message =
                        "One or more topics are invalid or do not belong to the selected domain."
                });
            }

            var difficultyExists = await _context.InterviewDifficulties
                .AnyAsync(x =>
                    x.Id == request.DifficultyId && x.IsActive == 1);

            if (!difficultyExists)
            {
                return BadRequest(new
                {
                    message = "Invalid interview difficulty."
                });
            }

            var durationExists = await _context.InterviewDurations
                .AnyAsync(x =>
                    x.Id == request.DurationId && x.IsActive == 1);

            if (!durationExists)
            {
                return BadRequest(new
                {
                    message = "Invalid interview duration."
                });
            }

            var personaExists = await _context.InterviewerPersonas
                .AnyAsync(x =>
                    x.Id == request.PersonaId && x.IsActive == 1);

            if (!personaExists)
            {
                return BadRequest(new
                {
                    message = "Invalid interviewer persona."
                });
            }

            var now = DateTime.UtcNow;

            var session = new InterviewSession
            {
                UserId = userId,
                DomainId = request.DomainId,
                DifficultyId = request.DifficultyId,
                DurationId = request.DurationId,
                PersonaId = request.PersonaId,
                Status = "Created",
                IsActive = 1,
                CreatedBy = userId,
                CreatedDate = now,
                UpdatedBy = userId,
                UpdatedDate = now,

                SessionTopics = request.TopicIds.Select(topicId =>
                    new InterviewSessionTopic
                    {
                        TopicId = topicId,
                        IsActive = 1,
                        CreatedBy = userId,
                        CreatedDate = now,
                        UpdatedBy = userId,
                        UpdatedDate = now
                    }).ToList()
            };

            _context.InterviewSessions.Add(session);
            await _context.SaveChangesAsync();

            var topicNames = await _context.Topics
                .Where(x => request.TopicIds.Contains(x.Id))
                .OrderBy(x => x.Name)
                .Select(x => x.Name)
                .ToListAsync();

            return Ok(new
            {
                message = "Interview session created successfully.",
                sessionId = session.Id,
                status = session.Status,
                domainId = session.DomainId,
                topicIds = request.TopicIds,
                topics = topicNames,
                difficultyId = session.DifficultyId,
                durationId = session.DurationId,
                personaId = session.PersonaId,
                createdDate = session.CreatedDate
            });
        }


        // GET: api/interview-sessions
        [HttpGet]
        public async Task<IActionResult> GetMySessions()
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var sessions = await _context.InterviewSessions
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.IsActive == 1)
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new
                {
                    x.Id,
                    x.Status,
                    x.CreatedDate,

                    Domain = x.Domain.Name,
                    Difficulty = x.Difficulty.Name,
                    DurationMinutes = x.Duration.DurationMinutes,
                    Persona = x.Persona.Name,

                    Topics = x.SessionTopics
                        .Where(st => st.IsActive == 1)
                        .Select(st => new
                        {
                            st.TopicId,
                            TopicName = st.Topic.Name
                        })
                        .OrderBy(t => t.TopicName)
                        .ToList()
                })
                .ToListAsync();

            return Ok(sessions);
        }

        // GET: api/interview-sessions/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSessionById(int id)
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var session = await _context.InterviewSessions
                .AsNoTracking()
                .Where(x =>
                    x.Id == id &&
                    x.UserId == userId &&
                    x.IsActive == 1)
                .Select(x => new
                {
                    x.Id,
                    x.Status,
                    x.CreatedDate,
                    x.UpdatedDate,

                    DomainId = x.DomainId,
                    Domain = x.Domain.Name,

                    DifficultyId = x.DifficultyId,
                    Difficulty = x.Difficulty.Name,

                    DurationId = x.DurationId,
                    DurationMinutes = x.Duration.DurationMinutes,

                    PersonaId = x.PersonaId,
                    Persona = x.Persona.Name,

                    Topics = x.SessionTopics
                        .Where(st => st.IsActive == 1)
                        .Select(st => new
                        {
                            st.TopicId,
                            TopicName = st.Topic.Name
                        })
                        .OrderBy(t => t.TopicName)
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (session == null)
            {
                // Also return 404 when the session belongs to another user.
                return NotFound(new
                {
                    message = "Interview session not found."
                });
            }

            return Ok(session);
        }


        // POST: api/interview-sessions/{id}/start
        [HttpPost("{id:int}/start")]
        public async Task<IActionResult> StartInterview(int id)
        {
            var userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub");

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            // Verify ownership and session status.
            var session = await _context.InterviewSessions
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.UserId == userId &&
                    x.IsActive == 1);

            if (session == null)
            {
                return NotFound(new
                {
                    message = "Interview session not found."
                });
            }

            if (session.Status != "Created")
            {
                return Conflict(new
                {
                    message = "Only a newly created session can be started.",
                    currentStatus = session.Status
                });
            }

            var now = DateTime.UtcNow;

            // Atomically claim the user's one-time free trial.
            var trialClaimed = await _context.Users
                .Where(x =>
                    x.Id == userId &&
                    x.IsActive == 1 &&
                    x.IsFreeTrialUsed == 0)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.IsFreeTrialUsed, 1)
                    .SetProperty(x => x.UpdatedBy, userId)
                    .SetProperty(x => x.UpdatedDate, now));

            if (trialClaimed == 0)
            {
                await transaction.RollbackAsync();

                return StatusCode(403, new
                {
                    message =
                        "Your free interview has already been used. Please subscribe to continue."
                });
            }

            // Start the session only if it is still in Created status.
            var sessionStarted = await _context.InterviewSessions
                .Where(x =>
                    x.Id == id &&
                    x.UserId == userId &&
                    x.IsActive == 1 &&
                    x.Status == "Created")
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Status, "InProgress")
                    .SetProperty(x => x.UpdatedBy, userId)
                    .SetProperty(x => x.UpdatedDate, now));

            if (sessionStarted == 0)
            {
                await transaction.RollbackAsync();

                return Conflict(new
                {
                    message = "The interview session could not be started."
                });
            }

            await transaction.CommitAsync();

            return Ok(new
            {
                message = "Interview started successfully.",
                sessionId = id,
                status = "InProgress",
                startedAt = now
            });
        }
    }
}