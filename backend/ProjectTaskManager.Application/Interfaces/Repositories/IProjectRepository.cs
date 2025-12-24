namespace ProjectTaskManager.Application.Interfaces.Repositories;

using ProjectTaskManager.Domain.Entities;

public interface IProjectRepository
{
    Task<List<Project>> GetUserProjectsAsync(Guid userId);
    Task<Project?> GetByIdAsync(Guid projectId);
    Task<Project?> GetByIdWithTasksAsync(Guid projectId);
    Task<Project?> GetByIdAndUserIdAsync(Guid projectId, Guid userId);
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(Project project);
    Task SaveChangesAsync();
}