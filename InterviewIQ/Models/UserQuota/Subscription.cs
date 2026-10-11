using InterviewIQ.Models.Entities;

namespace InterviewIQ.Models.UserQuota
{
    public class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string PlanName { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = "INR";
        public decimal PriceAmount { get; set; }

        public int InterviewCreditsGranted { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime CurrentPeriodStart { get; set; }
        public DateTime CurrentPeriodEnd { get; set; }

        public string Status { get; set; } = "Pending";
        public string? ProviderSubscriptionId { get; set; }

        public int IsActive { get; set; } = 1;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public User User { get; set; } = null!;
    }
}
