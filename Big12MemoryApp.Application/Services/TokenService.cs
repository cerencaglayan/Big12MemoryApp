using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Domain.Entities;
using Big12MemoryApp.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Big12MemoryApp.Application.Services;

public class TokenService
{
    private readonly IConfiguration _config;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public TokenService(IConfiguration config, IRefreshTokenRepository refreshTokenRepository)
    {
        _config = config;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public string CreateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                int.Parse(_config["Jwt:ExpireMinutes"]!)
            ),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> CreateRefreshTokenAsync(int userId, CancellationToken cancellationToken = default)
    {
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var entity = RefreshToken.Create(
            refreshToken,
            DateTime.UtcNow.AddDays(
                int.Parse(_config["Jwt:RefreshTokenExpireDays"]!)
            ), userId
        );

        await _refreshTokenRepository.AddAsync(entity, cancellationToken);

        return refreshToken;
    }


    public async Task<int> DeleteRefreshTokenAsync(int userId, CancellationToken cancellationToken = default)
    {
        var refreshTokens = await _refreshTokenRepository
            .GetByUserIdAsync(userId, cancellationToken);

        await _refreshTokenRepository.RemoveRangeAsync(refreshTokens, cancellationToken);

        return userId;
    }
}