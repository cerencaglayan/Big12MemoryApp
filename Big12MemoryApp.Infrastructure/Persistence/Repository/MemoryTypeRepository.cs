using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Domain.Entities.Lookup;
using Big12MemoryApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Big12MemoryApp.Infrastructure.Persistence.Repository;

public class MemoryTypeRepository : IMemoryTypeRepository
{
    private readonly ApplicationDbContext _dbContext;


    public MemoryTypeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MemoryType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MemoryTypes
            .Where(m => m.Id == id && !m.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<MemoryType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.MemoryTypes
            .Where(m => !m.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}