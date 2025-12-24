namespace ProjectTaskManager.Application.Services;

using ProjectTaskManager.Application.DTOs.Auth;
using ProjectTaskManager.Application.Interfaces;
using ProjectTaskManager.Application.Interfaces.Repositories;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
            return null;

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return null;

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            Email = user.Email,
            FullName = user.FullName
        };
    }
}