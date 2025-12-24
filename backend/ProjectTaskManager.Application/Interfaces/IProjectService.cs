namespace ProjectTaskManager.Application.Interfaces;

using ProjectTaskManager.Application.DTOs.Projects;

public interface IProjectService
{
    Task<List<ProjectResponse>> GetUserProjectsAsync(Guid userId);
    Task<ProjectDetailResponse?> GetProjectByIdAsync(Guid projectId, Guid userId);
    Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, Guid userId);
    Task<ProjectResponse?> UpdateProjectAsync(Guid projectId, UpdateProjectRequest request, Guid userId);
    Task<bool> DeleteProjectAsync(Guid projectId, Guid userId);
}