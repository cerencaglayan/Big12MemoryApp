using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Domain.Common;
using Big12MemoryApp.Domain.Entities.Lookup;
using Big12MemoryApp.Domain.Repositories;

namespace Big12MemoryApp.Application.Services;

public class MemoryTypeService(IMemoryTypeRepository memoryTypeRepository)
{
    public async Task<Result<List<MemoryType>>> getAllAsync(CancellationToken ct = default)
    {
        return await memoryTypeRepository.GetAllAsync(ct);
    }
}