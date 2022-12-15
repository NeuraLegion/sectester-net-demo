using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure;

public interface IUserContext
{
  DbSet<User> Users { get; }
  Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
