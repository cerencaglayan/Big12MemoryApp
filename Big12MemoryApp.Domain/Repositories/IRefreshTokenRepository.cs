using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Domain.Entities;

namespace Big12MemoryApp.Domain.Repositories;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken, int>
{
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);

    Task<List<RefreshToken>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    Task RemoveRangeAsync(IEnumerable<RefreshToken> refreshTokens, CancellationToken cancellationToken = default);
}