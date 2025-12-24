namespace ProjectTaskManager.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectTaskManager.Application.Interfaces;
using ProjectTaskManager.Application.Interfaces.Repositories;
using ProjectTaskManager.Infrastructure.Configuration;
using ProjectTaskManager.Infrastructure.Persistence;
using ProjectTaskManager.Infrastructure.Repositories;
using ProjectTaskManager.Infrastructure.Security;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // JWT Settings
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();

        // Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}