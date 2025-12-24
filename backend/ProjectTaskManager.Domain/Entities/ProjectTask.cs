namespace ProjectTaskManager.Domain.Entities;

public class ProjectTask
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Foreign key
    public Guid ProjectId { get; set; }

    // Navigation property
    public Project Project { get; set; } = null!;
}