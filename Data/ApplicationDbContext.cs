using AishaTulegen.ViewModelValidation.Models;
using Microsoft.EntityFrameworkCore;

namespace AishaTulegen.ViewModelValidation.Data;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<User>();
        user.HasKey(item => item.Id);
        user.HasIndex(item => item.Email).IsUnique();
        user.Property(item => item.FullName).HasMaxLength(100).IsRequired();
        user.Property(item => item.Email).HasMaxLength(254).IsRequired();
        user.Property(item => item.PasswordHash).IsRequired();
        user.Property(item => item.CreatedAt).IsRequired();
    }
}
