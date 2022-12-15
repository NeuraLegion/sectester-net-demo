using Microsoft.EntityFrameworkCore;
using Polly;

namespace App.Infrastructure;

public static class ServiceProviderExtensions
{
  public static IServiceProvider MigrateDbContext<TContext>(this IServiceProvider sp, Action<TContext, IServiceProvider> seeder)
    where TContext : DbContext
  {
    using var scope = sp.CreateScope();
    var services = scope.ServiceProvider;

    var logger = services.GetRequiredService<ILogger<TContext>>();

    var context = services.GetService<TContext>();

    try
    {
      logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

      var retry = Policy.Handle<Exception>()
        .WaitAndRetry(new[]
        {
          TimeSpan.FromSeconds(3),
          TimeSpan.FromSeconds(5),
          TimeSpan.FromSeconds(8)
        });

      retry.Execute(() => InvokeSeeder(seeder!, context, services));

      logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
    }

    return sp;
  }

  private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services)
    where TContext : DbContext
  {
    context.Database.Migrate();
    seeder(context, services);
  }
}
