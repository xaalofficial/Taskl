namespace ProjectTaskManager.Application.DTOs.Projects;

public class ProjectDetailResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ProjectProgressResponse Progress { get; set; } = new();
}