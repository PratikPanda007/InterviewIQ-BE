using InterviewIQ.Models.Entities;

namespace InterviewIQ.Models.UserQuota
{
    public class UserMonthlyUsage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int FreeInterviewsUsed { get; set; } = 0;

        public int IsActive { get; set; } = 1;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public User User { get; set; } = null!;
    }
}
