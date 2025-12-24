namespace ProjectTaskManager.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Foreign key
    public Guid UserId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();
}