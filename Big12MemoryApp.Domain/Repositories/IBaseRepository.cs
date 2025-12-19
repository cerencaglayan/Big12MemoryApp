using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Domain.Entities.Base;

namespace Big12MemoryApp.Domain.Repositories;

public interface IBaseRepository<TEntity, TType> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(TType id, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}