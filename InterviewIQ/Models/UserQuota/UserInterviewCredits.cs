using InterviewIQ.Models.Entities;

namespace InterviewIQ.Models.UserQuota
{
    public class UserInterviewCredits
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GrantedCredits { get; set; }
        public int RemainingCredits { get; set; }
        public int? PaymentTransactionId { get; set; }

        public int IsActive { get; set; } = 1;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public User User { get; set; } = null!;
    }
}
