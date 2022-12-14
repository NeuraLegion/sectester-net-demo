using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure;

public class UserContext : DbContext
{
  public UserContext(DbContextOptions<UserContext> options) : base(options)
  {
  }

  public DbSet<User> Users { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
    optionsBuilder.UseNpgsql(
      $"Host=localhost;Username={Environment.GetEnvironmentVariable("POSTGRES_USER")};Password={Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")};Database=test");

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<User>()
      .Property(u => u.Name)
      .IsRequired();

    modelBuilder.Entity<User>()
      .Property(u => u.IsActive)
      .HasDefaultValue(true);
  }
}
