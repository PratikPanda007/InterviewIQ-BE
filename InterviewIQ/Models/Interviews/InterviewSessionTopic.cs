using InterviewIQ.Models.Entities;

namespace InterviewIQ.Models.Interviews
{
    public class InterviewSessionTopic
    {
        public int Id { get; set; }

        public int InterviewSessionId { get; set; }
        public int TopicId { get; set; }

        public int IsActive { get; set; } = 1;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public InterviewSession InterviewSession { get; set; } = null!;
        public Topic Topic { get; set; } = null!;
    }
}
