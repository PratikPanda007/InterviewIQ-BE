namespace InterviewIQ.Models.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; }
        = new List<User>();
    public int IsActive { get; set; } = 1;
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int UpdatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
}