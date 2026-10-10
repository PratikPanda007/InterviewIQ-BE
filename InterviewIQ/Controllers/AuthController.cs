using InterviewIQ.Data;
using InterviewIQ.Models.Auth;
using InterviewIQ.Models.Configurations;
using InterviewIQ.Models.Entities;
using InterviewIQ.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InterviewIQ.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly InterviewIQDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly IEmailService _emailService;
    private readonly JwtService _jwtService;
    private readonly RefreshTokenService _refreshTokenService;
    private readonly PasswordResetService _passwordResetService;
    private readonly Microsoft.Extensions.Options.IOptions<FrontendSettings> _frontendSettings;

    public AuthController(InterviewIQDbContext context, PasswordService passwordService, IEmailService emailService, JwtService jwtService,
        RefreshTokenService refreshTokenService, PasswordResetService passwordResetService, IOptions<FrontendSettings> frontendSettings)
    {
        _context = context;
        _passwordService = passwordService;
        _emailService = emailService;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
        _passwordResetService = passwordResetService;
        _frontendSettings = frontendSettings;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var email = request.Email.Trim().ToLower();

        var user = await _context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Email == email);

        if (user == null || user.IsActive != 1)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var isPasswordValid = _passwordService.VerifyPassword(
            request.Password,
            user.PasswordHash);

        if (!isPasswordValid)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var accessToken = _jwtService.GenerateToken(user);

        var refreshToken = _refreshTokenService.GenerateToken(user.Id);

        _context.RefreshTokens.Add(refreshToken);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Login successful.",
            accessToken,
            refreshToken = refreshToken.Token,
            user = new
            {
                id = user.Id,
                fullName = user.FullName,
                email = user.Email,
                role = user.Role.Name
            }
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Unauthorized(new
            {
                message = "Refresh token is required."
            });
        }

        var refreshToken = await _context.RefreshTokens
            .Include(x => x.User)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x =>
                x.Token == request.RefreshToken);

        if (refreshToken == null)
        {
            return Unauthorized(new
            {
                message = "Invalid refresh token."
            });
        }

        if (refreshToken.IsActive != 1)
        {
            return Unauthorized(new
            {
                message = "Refresh token is inactive."
            });
        }

        if (refreshToken.RevokedAt.HasValue)
        {
            return Unauthorized(new
            {
                message = "Refresh token has been revoked."
            });
        }

        if (refreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            return Unauthorized(new
            {
                message = "Refresh token has expired."
            });
        }

        if (refreshToken.User.IsActive != 1)
        {
            return Unauthorized(new
            {
                message = "User account is inactive."
            });
        }

        // Revoke the old refresh token
        refreshToken.IsActive = 0;
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.UpdatedDate = DateTime.UtcNow;
        refreshToken.UpdatedBy = refreshToken.UserId;

        // Generate a new access token
        var accessToken = _jwtService.GenerateToken(
            refreshToken.User);

        // Generate a new refresh token
        var newRefreshToken = _refreshTokenService.GenerateToken(
            refreshToken.UserId);

        _context.RefreshTokens.Add(newRefreshToken);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Token refreshed successfully.",
            accessToken,
            refreshToken = newRefreshToken.Token
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
    RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return BadRequest(new
            {
                message = "Refresh token is required."
            });
        }

        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x =>
                x.Token == request.RefreshToken);

        // Return success even if the token does not exist
        // or has already been revoked.
        if (refreshToken == null ||
            refreshToken.RevokedAt.HasValue ||
            refreshToken.IsActive != 1)
        {
            return Ok(new
            {
                message = "Logged out successfully."
            });
        }

        var now = DateTime.UtcNow;

        refreshToken.IsActive = 0;
        refreshToken.RevokedAt = now;
        refreshToken.UpdatedDate = now;
        refreshToken.UpdatedBy = refreshToken.UserId;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Logged out successfully."
        });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
    ForgotPasswordRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.Email == email && x.IsActive == 1);

        // Use the same response whether or not the account exists.
        if (user != null)
        {
            var token = await _passwordResetService.CreateTokenAsync(user);

            var resetUrl = _frontendSettings.Value.ResetPasswordUrl;

            var resetLink =
                $"{resetUrl}?token={Uri.EscapeDataString(token)}";

            await _emailService.SendPasswordResetEmailAsync(
                user.Email,
                user.FullName,
                resetLink);
        }

        return Ok(new
        {
            message = "If an active account exists for this email, a password reset link will be sent."
        });
    }

    [HttpPost("reset-password")]
public async Task<IActionResult> ResetPassword(
    ResetPasswordRequest request)
{
    var tokenHash = PasswordResetService.HashToken(request.Token);

    var resetToken = await _context.PasswordResetTokens
        .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

    if (resetToken == null ||
        resetToken.IsActive != 1 ||
        resetToken.UsedAt.HasValue ||
        resetToken.ExpiresAt <= DateTime.UtcNow)
    {
        return BadRequest(new
        {
            message = "This password reset link is invalid or has expired."
        });
    }

    var user = await _context.Users
        .FirstOrDefaultAsync(x =>
            x.Id == resetToken.UserId && x.IsActive == 1);

    if (user == null)
    {
        return BadRequest(new
        {
            message = "This password reset link is invalid or has expired."
        });
    }

    var now = DateTime.UtcNow;

    user.PasswordHash =
        _passwordService.HashPassword(request.NewPassword);

    user.UpdatedBy = user.Id;
    user.UpdatedDate = now;

    resetToken.UsedAt = now;
    resetToken.IsActive = 0;
    resetToken.UpdatedBy = user.Id;
    resetToken.UpdatedDate = now;

    // Revoke all refresh tokens for this user.
    var activeRefreshTokens = await _context.RefreshTokens
        .Where(x => x.UserId == user.Id && x.IsActive == 1)
        .ToListAsync();

    foreach (var refreshToken in activeRefreshTokens)
    {
        refreshToken.IsActive = 0;
        refreshToken.RevokedAt = now;
        refreshToken.UpdatedBy = user.Id;
        refreshToken.UpdatedDate = now;
    }

    await _context.SaveChangesAsync();

    return Ok(new
    {
        message = "Password reset successfully. Please log in with your new password."
    });
}
}