namespace ProjectTaskManager.Application.DTOs.Tasks;

public class TaskFilterRequest
{
    public string? SearchTerm { get; set; }
    public bool? IsCompleted { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}