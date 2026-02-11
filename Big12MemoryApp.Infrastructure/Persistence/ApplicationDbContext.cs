using Big12MemoryApp.Domain.Entities;
using Big12MemoryApp.Domain.Entities.Lookup;
using Microsoft.EntityFrameworkCore;

namespace Big12MemoryApp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Memory> Memories { get; set; }
    public DbSet<MemoryType> MemoryTypes { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<MemoryAttachment> MemoryAttachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>();
        modelBuilder.Entity<Memory>();
        modelBuilder.Entity<MemoryType>();
        modelBuilder.Entity<RefreshToken>();
        modelBuilder.Entity<Attachment>();
        modelBuilder.Entity<MemoryAttachment>();
    }
}