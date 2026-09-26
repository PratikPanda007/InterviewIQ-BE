using InterviewIQ.Data;
using InterviewIQ.Models.Auth;
using InterviewIQ.Models.Entities;
using InterviewIQ.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewIQ.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly InterviewIQDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly IEmailService _emailService;

    public AuthController(
        InterviewIQDbContext context,
        PasswordService passwordService,
        IEmailService emailService)
    {
        _context = context;
        _passwordService = passwordService;
        _emailService = emailService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLower();

        var existingUser = await _context.Users
            .AnyAsync(x => x.Email == email);

        if (existingUser)
        {
            return Conflict(new
            {
                message = "An account with this email already exists."
            });
        }

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = email,
            PasswordHash = _passwordService.HashPassword(request.Password),

            RoleId = 3,
            IsFreeTrialUsed = 0,
            IsActive = 1,

            CreatedBy = 0,
            CreatedDate = DateTime.UtcNow,

            UpdatedBy = 0,
            UpdatedDate = DateTime.UtcNow
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        await _emailService.SendWelcomeEmailAsync(user.Email, user.FullName);

        return Ok(new
        {
            message = "Registration successful.",
            userId = user.Id
        });
    }
}