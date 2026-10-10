namespace InterviewIQ.Models.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public InterviewDomain Domain { get; set; } = null!;
    }
}
