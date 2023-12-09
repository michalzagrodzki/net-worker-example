using Microsoft.EntityFrameworkCore;
using MediatR;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<LocalDbContext>();

            // Ensure the database is created and migrated
            await dbContext.Database.EnsureCreatedAsync();
        }

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<LocalDbContext>(options =>
                    options.UseInMemoryDatabase(databaseName: "NumberDatabase"));

                services.AddMediatR(typeof(Program));
                services.AddScoped<NumberRepository>();
                services.AddHostedService<NumberBackgroundService>();
                services.AddLogging();
            });
}
