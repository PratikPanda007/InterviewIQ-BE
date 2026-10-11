using InterviewIQ.Models.Entities;

namespace InterviewIQ.Models.UserQuota
{
    public class PaymentTransaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? SubscriptionId { get; set; }

        public string Provider { get; set; } = "Razorpay";
        public string CurrencyCode { get; set; } = "INR";
        public decimal Amount { get; set; }

        public string Status { get; set; } = "Pending";
        public string? ProviderOrderId { get; set; }
        public string? ProviderPaymentId { get; set; }
        public string? IdempotencyKey { get; set; }

        public int IsActive { get; set; } = 1;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public User User { get; set; } = null!;
        public Subscription? Subscription { get; set; }
    }
}
