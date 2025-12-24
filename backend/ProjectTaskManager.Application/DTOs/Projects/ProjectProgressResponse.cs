namespace ProjectTaskManager.Application.DTOs.Projects;

public class ProjectProgressResponse
{
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public decimal ProgressPercentage { get; set; }
}