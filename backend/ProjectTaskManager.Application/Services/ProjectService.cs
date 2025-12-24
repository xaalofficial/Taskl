namespace ProjectTaskManager.Application.Services;

using ProjectTaskManager.Application.DTOs.Projects;
using ProjectTaskManager.Application.Interfaces;
using ProjectTaskManager.Application.Interfaces.Repositories;
using ProjectTaskManager.Domain.Entities;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<List<ProjectResponse>> GetUserProjectsAsync(Guid userId)
    {
        var projects = await _projectRepository.GetUserProjectsAsync(userId);

        return projects.Select(p => new ProjectResponse
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();
    }

    public async Task<ProjectDetailResponse?> GetProjectByIdAsync(Guid projectId, Guid userId)
    {
        var project = await _projectRepository.GetByIdWithTasksAsync(projectId);

        if (project == null || project.UserId != userId)
            return null;

        return new ProjectDetailResponse
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
            Progress = new ProjectProgressResponse
            {
                TotalTasks = project.Tasks.Count,
                CompletedTasks = project.Tasks.Count(t => t.IsCompleted),
                ProgressPercentage = project.Tasks.Count == 0 ? 0 :
                    Math.Round((decimal)project.Tasks.Count(t => t.IsCompleted) / project.Tasks.Count * 100, 2)
            }
        };
    }

    public async Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request, Guid userId)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _projectRepository.AddAsync(project);
        await _projectRepository.SaveChangesAsync();

        return new ProjectResponse
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }

    public async Task<ProjectResponse?> UpdateProjectAsync(Guid projectId, UpdateProjectRequest request, Guid userId)
    {
        var project = await _projectRepository.GetByIdAndUserIdAsync(projectId, userId);

        if (project == null)
            return null;

        project.Title = request.Title;
        project.Description = request.Description;
        project.UpdatedAt = DateTime.UtcNow;

        await _projectRepository.UpdateAsync(project);
        await _projectRepository.SaveChangesAsync();

        return new ProjectResponse
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }

    public async Task<bool> DeleteProjectAsync(Guid projectId, Guid userId)
    {
        var project = await _projectRepository.GetByIdAndUserIdAsync(projectId, userId);

        if (project == null)
            return false;

        await _projectRepository.DeleteAsync(project);
        await _projectRepository.SaveChangesAsync();

        return true;
    }
}