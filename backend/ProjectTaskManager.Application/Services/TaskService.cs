namespace ProjectTaskManager.Application.Services;

using ProjectTaskManager.Application.DTOs.Tasks;
using ProjectTaskManager.Application.Interfaces;
using ProjectTaskManager.Application.Interfaces.Repositories;
using ProjectTaskManager.Domain.Common;
using ProjectTaskManager.Domain.Entities;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<PagedResult<TaskResponse>> GetTasksAsync(Guid projectId, Guid userId, TaskFilterRequest filter)
    {
        // Verify project belongs to user
        var projectBelongsToUser = await _taskRepository.ProjectBelongsToUserAsync(projectId, userId);

        if (!projectBelongsToUser)
            return new PagedResult<TaskResponse>();

        var result = await _taskRepository.GetTasksAsync(
            projectId,
            filter.SearchTerm,
            filter.IsCompleted,
            filter.PageNumber,
            filter.PageSize);

        return new PagedResult<TaskResponse>
        {
            Items = result.Items.Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt,
                CompletedAt = t.CompletedAt
            }).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }

    public async Task<TaskResponse?> GetTaskByIdAsync(Guid taskId, Guid userId)
    {
        var task = await _taskRepository.GetByIdWithProjectAsync(taskId);

        if (task == null || task.Project.UserId != userId)
            return null;

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            CompletedAt = task.CompletedAt
        };
    }

    public async Task<TaskResponse> CreateTaskAsync(Guid projectId, CreateTaskRequest request, Guid userId)
    {
        // Verify project belongs to user
        var project = await _projectRepository.GetByIdAndUserIdAsync(projectId, userId);

        if (project == null)
            throw new UnauthorizedAccessException("Project not found or access denied");

        var task = new ProjectTask
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            ProjectId = projectId,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _taskRepository.AddAsync(task);
        await _taskRepository.SaveChangesAsync();

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            CompletedAt = task.CompletedAt
        };
    }

    public async Task<TaskResponse?> UpdateTaskAsync(Guid taskId, UpdateTaskRequest request, Guid userId)
    {
        var task = await _taskRepository.GetByIdWithProjectAsync(taskId);

        if (task == null || task.Project.UserId != userId)
            return null;

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueDate = request.DueDate;

        await _taskRepository.UpdateAsync(task);
        await _taskRepository.SaveChangesAsync();

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            CompletedAt = task.CompletedAt
        };
    }

    public async Task<bool> MarkTaskAsCompletedAsync(Guid taskId, Guid userId)
    {
        var task = await _taskRepository.GetByIdWithProjectAsync(taskId);

        if (task == null || task.Project.UserId != userId)
            return false;

        task.IsCompleted = true;
        task.CompletedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);
        await _taskRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteTaskAsync(Guid taskId, Guid userId)
    {
        var task = await _taskRepository.GetByIdWithProjectAsync(taskId);

        if (task == null || task.Project.UserId != userId)
            return false;

        await _taskRepository.DeleteAsync(task);
        await _taskRepository.SaveChangesAsync();

        return true;
    }
}