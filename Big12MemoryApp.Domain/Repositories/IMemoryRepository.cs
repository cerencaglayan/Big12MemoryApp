using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Domain.Entities;

namespace Big12MemoryApp.Domain.Repositories;

public interface IMemoryRepository : IBaseRepository<Memory, int>
{
    Task<Memory?> GetByIdWithAttachmentsAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Memory>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<Memory> CreateAsync(Memory memory, CancellationToken cancellationToken = default);
    Task UpdateAsync(Memory memory, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}