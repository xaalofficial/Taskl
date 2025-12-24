namespace ProjectTaskManager.Application.Interfaces.Repositories;

using ProjectTaskManager.Domain.Entities;
using ProjectTaskManager.Domain.Common;

public interface ITaskRepository
{
    Task<PagedResult<ProjectTask>> GetTasksAsync(Guid projectId, string? searchTerm, bool? isCompleted, int pageNumber, int pageSize);
    Task<ProjectTask?> GetByIdAsync(Guid taskId);
    Task<ProjectTask?> GetByIdWithProjectAsync(Guid taskId);
    Task<bool> ProjectBelongsToUserAsync(Guid projectId, Guid userId);
    Task AddAsync(ProjectTask task);
    Task UpdateAsync(ProjectTask task);
    Task DeleteAsync(ProjectTask task);
    Task SaveChangesAsync();
}