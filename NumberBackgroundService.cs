using MediatR;

public class NumberBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NumberBackgroundService> _logger;

    public NumberBackgroundService(IServiceProvider serviceProvider, ILogger<NumberBackgroundService> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        while (!stoppingToken.IsCancellationRequested)
        {
            // Generate a random number
            var randomNumber = new Random().Next(1, 100); // Replace with your own logic to generate a random number

            try
            {
                // Send the number to MediatR handler through request
                var result = await mediator.Send(randomNumber, stoppingToken);

                // Log the response from MediatR
                _logger.LogInformation($"Number saved to database: {result}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving number to database");
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken); // Adjust delay as needed
        }
    }
}