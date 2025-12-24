namespace ProjectTaskManager.Application.Interfaces;

using ProjectTaskManager.Application.DTOs.Auth;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}