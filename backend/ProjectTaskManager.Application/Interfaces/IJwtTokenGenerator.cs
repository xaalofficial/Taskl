namespace ProjectTaskManager.Application.Interfaces;

using ProjectTaskManager.Domain.Entities;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}       