using Identity.Service.Models;
using Microsoft.EntityFrameworkCore;
namespace Identity.Service.Services;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("User");
            e.HasKey(u => u.UserId);
            e.Property(u => u.UserId).HasColumnName("UserId").ValueGeneratedOnAdd();
            e.Property(u => u.Username).HasColumnName("Username").HasMaxLength(50).IsRequired();
            e.Property(u => u.PasswordHash).HasColumnName("PasswordHash").HasMaxLength(255).IsRequired();
            e.Property(u => u.Role).HasColumnName("Role").HasMaxLength(20).IsRequired();
            e.Property(u => u.RefreshToken).HasColumnName("RefreshToken").HasMaxLength(500);
            e.Property(u => u.RefreshTokenExpiry).HasColumnName("RefreshTokenExpiry");
            e.HasIndex(u => u.Username).IsUnique();
        });
    }
}
