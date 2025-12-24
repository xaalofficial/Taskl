namespace ProjectTaskManager.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using ProjectTaskManager.Application.Interfaces;
using ProjectTaskManager.Domain.Entities;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        // Apply pending migrations
        await context.Database.MigrateAsync();

        // Check if already seeded
        if (await context.Users.AnyAsync())
            return;

        // Seed test user
        var testUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            PasswordHash = passwordHasher.HashPassword("Test123!"),
            FullName = "Test User",
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(testUser);
        await context.SaveChangesAsync();
    }
}