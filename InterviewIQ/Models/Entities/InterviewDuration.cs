namespace InterviewIQ.Models.Entities
{
    public class InterviewDuration
    {
        public int Id { get; set; }
        public int DurationMinutes { get; set; }
        public int SortOrder { get; set; }

        public int IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
