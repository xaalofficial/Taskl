namespace ProjectTaskManager.Application.Interfaces.Repositories;

using ProjectTaskManager.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}