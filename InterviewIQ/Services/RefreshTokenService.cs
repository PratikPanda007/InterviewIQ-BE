using System.Security.Cryptography;
using InterviewIQ.Models.Configurations;
using InterviewIQ.Models.Entities;
using Microsoft.Extensions.Options;

namespace InterviewIQ.Services;

public class RefreshTokenService
{
    private readonly RefreshTokenSettings _settings;

    public RefreshTokenService(
        IOptions<RefreshTokenSettings> settings)
    {
        _settings = settings.Value;
    }

    public RefreshToken GenerateToken(int userId)
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);

        var token = Convert.ToBase64String(tokenBytes);

        var now = DateTime.UtcNow;

        return new RefreshToken
        {
            UserId = userId,
            Token = token,
            ExpiresAt = now.AddDays(_settings.ExpirationDays),
            RevokedAt = null,
            IsActive = 1,
            CreatedDate = now,
            CreatedBy = userId,
            UpdatedDate = now,
            UpdatedBy = userId
        };
    }
}