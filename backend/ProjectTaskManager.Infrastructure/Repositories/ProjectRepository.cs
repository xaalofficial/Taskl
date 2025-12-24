namespace ProjectTaskManager.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ProjectTaskManager.Application.Interfaces.Repositories;
using ProjectTaskManager.Domain.Entities;
using ProjectTaskManager.Infrastructure.Persistence;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetUserProjectsAsync(Guid userId)
    {
        return await _context.Projects
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid projectId)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task<Project?> GetByIdWithTasksAsync(Guid projectId)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }

    public async Task<Project?> GetByIdAndUserIdAsync(Guid projectId, Guid userId)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }

    public async Task UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
    }

    public async Task DeleteAsync(Project project)
    {
        _context.Projects.Remove(project);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}