using InterviewIQ.Data;
using InterviewIQ.Models.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InterviewIQ.Services
{
    public class PasswordResetService
    {
        private readonly InterviewIQDbContext _context;

        public PasswordResetService(InterviewIQDbContext context)
        {
            _context = context;
        }

        public static string HashToken(string token)
        {
            var hash = SHA256.HashData(
                Encoding.UTF8.GetBytes(token));

            return Convert.ToHexString(hash);
        }

        public async Task<string> CreateTokenAsync(User user)
        {
            var now = DateTime.UtcNow;

            var previousTokens = await _context.PasswordResetTokens
                .Where(x => x.UserId == user.Id && x.IsActive == 1)
                .ToListAsync();

            foreach (var previous in previousTokens)
            {
                previous.IsActive = 0;
                previous.UpdatedBy = user.Id;
                previous.UpdatedDate = now;
            }

            var rawBytes = RandomNumberGenerator.GetBytes(32);

            var token = Convert.ToBase64String(rawBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');

            _context.PasswordResetTokens.Add(new PasswordResetToken
            {
                UserId = user.Id,
                TokenHash = HashToken(token),
                ExpiresAt = now.AddMinutes(10),
                UsedAt = null,
                IsActive = 1,
                CreatedBy = user.Id,
                CreatedDate = now,
                UpdatedBy = user.Id,
                UpdatedDate = now
            });

            await _context.SaveChangesAsync();

            return token;
        }
    }
}
