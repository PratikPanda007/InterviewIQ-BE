using System.Data;

namespace InterviewIQ.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public Role? Role { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; }
        = new List<RefreshToken>();
    public int IsFreeTrialUsed { get; set; } = 0;
    public int IsActive { get; set; } = 1;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
}