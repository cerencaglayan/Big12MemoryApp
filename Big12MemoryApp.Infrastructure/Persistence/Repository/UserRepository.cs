using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Big12MemoryApp.Domain.Entities;
using Big12MemoryApp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Big12MemoryApp.Infrastructure.Persistence.Repository;

public class UserRepository(ApplicationDbContext dbContext):IUserRepository
{
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<User>()
            .Where(q => q.Id == id && !q.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<User>()
            .Where(q => !q.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<User>()
            .Where(q => q.Name == name && !q.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<User?>> GetByBirthdayAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<User>()
            .Where(q =>
                q.Birthday.HasValue &&              
                !q.IsDeleted &&                  
                q.Birthday.Value.Day == date.Day && 
                q.Birthday.Value.Month == date.Month
            )
            .ToListAsync(cancellationToken);
    }

    
    
    
    
}