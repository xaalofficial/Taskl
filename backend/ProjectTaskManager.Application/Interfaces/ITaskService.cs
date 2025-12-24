namespace ProjectTaskManager.Application.Interfaces;

using ProjectTaskManager.Application.DTOs.Tasks;
using ProjectTaskManager.Domain.Common;

public interface ITaskService
{
    Task<PagedResult<TaskResponse>> GetTasksAsync(Guid projectId, Guid userId, TaskFilterRequest filter);
    Task<TaskResponse?> GetTaskByIdAsync(Guid taskId, Guid userId);
    Task<TaskResponse> CreateTaskAsync(Guid projectId, CreateTaskRequest request, Guid userId);
    Task<TaskResponse?> UpdateTaskAsync(Guid taskId, UpdateTaskRequest request, Guid userId);
    Task<bool> MarkTaskAsCompletedAsync(Guid taskId, Guid userId);
    Task<bool> DeleteTaskAsync(Guid taskId, Guid userId);
}