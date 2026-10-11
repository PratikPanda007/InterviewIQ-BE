using System.ComponentModel.DataAnnotations;

namespace InterviewIQ.Models.Interviews
{
    public class CreateInterviewSessionRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int DomainId { get; set; }

        [Required]
        [MinLength(1)]
        public List<int> TopicIds { get; set; } = new();

        [Required]
        [Range(1, int.MaxValue)]
        public int DifficultyId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DurationId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PersonaId { get; set; }
    }
}
