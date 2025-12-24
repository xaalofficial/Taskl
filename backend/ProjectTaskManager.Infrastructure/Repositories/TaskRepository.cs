namespace ProjectTaskManager.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ProjectTaskManager.Application.Interfaces.Repositories;
using ProjectTaskManager.Domain.Common;
using ProjectTaskManager.Domain.Entities;
using ProjectTaskManager.Infrastructure.Persistence;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<ProjectTask>> GetTasksAsync(
        Guid projectId,
        string? searchTerm,
        bool? isCompleted,
        int pageNumber,
        int pageSize)
    {
        var query = _context.Tasks
            .Where(t => t.ProjectId == projectId);

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => t.Title.Contains(searchTerm));
        }

        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }

        var totalCount = await query.CountAsync();

        var tasks = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<ProjectTask>
        {
            Items = tasks,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<ProjectTask?> GetByIdAsync(Guid taskId)
    {
        return await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<ProjectTask?> GetByIdWithProjectAsync(Guid taskId)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<bool> ProjectBelongsToUserAsync(Guid projectId, Guid userId)
    {
        return await _context.Projects
            .AnyAsync(p => p.Id == projectId && p.UserId == userId);
    }

    public async Task AddAsync(ProjectTask task)
    {
        await _context.Tasks.AddAsync(task);
    }

    public async Task UpdateAsync(ProjectTask task)
    {
        _context.Tasks.Update(task);
    }

    public async Task DeleteAsync(ProjectTask task)
    {
        _context.Tasks.Remove(task);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}