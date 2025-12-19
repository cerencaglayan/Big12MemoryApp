using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Application.DTO.Requests;
using Big12MemoryApp.Application.DTO.Responses;
using Big12MemoryApp.Domain.Common;
using Big12MemoryApp.Domain.Common.Error;
using Big12MemoryApp.Domain.Repositories;
using Big12MemoryApp.Domain.Services;

namespace Big12MemoryApp.Application.Services;

public class UserService(TokenService tokenService, IUserRepository userRepository)
{
    
    public async Task<Result<LoginResponse>> Login(LoginRequest request, CancellationToken ct = default)
    {
        var user = await userRepository.GetByNameAsync(request.Name, ct);

        if (user == null)
            return Result<LoginResponse>.Failure(UserErrors.UserNotfound(request.Name));

        var isPasswordValid = user.Password != null && PasswordHasher.Verify(
            request.Password,
            user.Password
        );

        if (!isPasswordValid)
            return Result<LoginResponse>.Failure(UserErrors.InvalidCurrentPassword());

        var accessToken = tokenService.CreateAccessToken(user);
        var refreshToken = await tokenService.CreateRefreshTokenAsync(user.Id);

        var response = new LoginResponse
        {
            accessToken = accessToken,
            refreshToken = refreshToken
        };

        return Result<LoginResponse>.Success(response);
    }

    public async Task<Result<bool>> Logout(int userId, CancellationToken ct = default)
    {
        await tokenService.DeleteRefreshTokenAsync(userId);
        
        return Result<bool>.Success(true);
    }

}