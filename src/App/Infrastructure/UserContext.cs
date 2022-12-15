using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure;

public class UserContext : DbContext, IUserContext
{
  public UserContext(DbContextOptions<UserContext> options) : base(options)
  {
  }

  public virtual DbSet<User> Users { get; set; }

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
