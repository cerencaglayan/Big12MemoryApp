using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Domain.Entities;
using Big12MemoryApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Big12MemoryApp.Infrastructure.Persistence.Repository;

public class RefreshTokenRepository(ApplicationDbContext dbContext) : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.RefreshTokens
            .Where(x => x.UserId == id && !x.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<RefreshToken>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.RefreshTokens
            .Where(x => !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<RefreshToken>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.RefreshTokens
            .Where(x => x.UserId == userId && !x.IsRevoked && !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task RemoveRangeAsync(IEnumerable<RefreshToken> refreshTokens, CancellationToken cancellationToken = default)
    {
        dbContext.RefreshTokens.RemoveRange(refreshTokens);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}


