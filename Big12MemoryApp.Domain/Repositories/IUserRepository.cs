using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Domain.Entities;

namespace Big12MemoryApp.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User,int>
{
    Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<List<User?>> GetByBirthdayAsync(DateTime date, CancellationToken cancellationToken = default);

}