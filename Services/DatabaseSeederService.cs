using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DataTransform.Data;
using DataTransform.Models;

namespace DataTransform.Services
{
    public class DatabaseSeederService(
        IServiceProvider serviceProvider,
        ILogger<DatabaseSeederService> logger)
        : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting database seeder service");

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Check if database exists, if not create it
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);

            // Check if there's any data in the raw_user_events table
            if (!await dbContext.RawUserEvents.AnyAsync(cancellationToken))
            {
                logger.LogInformation("Seeding raw user events data");
                await SeedRawDataAsync(dbContext, cancellationToken);
                logger.LogInformation("Raw user events data seeded successfully");
            }
            else
            {
                logger.LogInformation("Raw user events data already exists, skipping seeding");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping database seeder service");
            return Task.CompletedTask;
        }

        private async Task SeedRawDataAsync(AppDbContext context, CancellationToken cancellationToken)
        {
            var rawEvents = new List<RawUserEvent>();
            await context.RawUserEvents.AddRangeAsync(rawEvents, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
