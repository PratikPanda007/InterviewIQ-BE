using InterviewIQ.Models.Entities;

namespace InterviewIQ.Models.Interviews
{
    public class InterviewSession
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int DomainId { get; set; }
        public int DifficultyId { get; set; }
        public int DurationId { get; set; }
        public int PersonaId { get; set; }

        public string Status { get; set; } = "Created";

        public int IsActive { get; set; } = 1;
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public User User { get; set; } = null!;
        public InterviewDomain Domain { get; set; } = null!;
        public InterviewDifficulty Difficulty { get; set; } = null!;
        public InterviewDuration Duration { get; set; } = null!;
        public InterviewerPersona Persona { get; set; } = null!;

        public ICollection<InterviewSessionTopic> SessionTopics { get; set; }
            = new List<InterviewSessionTopic>();
    }
}
