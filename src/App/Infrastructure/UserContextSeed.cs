using App.Models;
using Polly;
using Polly.Retry;

namespace App.Infrastructure;

public class UserContextSeed
{
  public async Task SeedAsync(UserContext context, IWebHostEnvironment env,
    ILogger<UserContextSeed> logger)
  {
    var policy = CreatePolicy(logger, nameof(UserContextSeed));

    await policy.ExecuteAsync(async () =>
    {
      if (!context.Users.Any())
      {
        await context.Users.AddRangeAsync(GetPreconfiguredItems()).ConfigureAwait(false);

        await context.SaveChangesAsync().ConfigureAwait(false);
      }
    }).ConfigureAwait(false);
  }

  private IEnumerable<User> GetPreconfiguredItems() =>
    new List<User>
    {
      new() { Name = "Van Owen" },
      new() { Name = "Kolton Ballard" },
      new() { Name = "Ty Sandoval" },
      new() { Name = "Melissa Jordan" }
    };

  private AsyncRetryPolicy CreatePolicy(ILogger<UserContextSeed> logger, string prefix, int retries = 3) =>
    Policy.Handle<Exception>().WaitAndRetryAsync(
      retries,
      _ => TimeSpan.FromSeconds(5),
      (exception, _, retry, _) => logger.LogWarning(exception,
        "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix,
        exception.GetType().Name, exception.Message, retry, retries));
}