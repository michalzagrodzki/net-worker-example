using MediatR;

public class NumberBackgroundService : BackgroundService
{
    private readonly IMediator _mediator;
    private readonly ILogger<NumberBackgroundService> _logger;

    public NumberBackgroundService(IMediator mediator, ILogger<NumberBackgroundService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Generate a random number
            var randomNumber = new Random().Next(1, 100); // Replace with your own logic to generate a random number

            try
            {
                // Send the number to MediatR handler through request
                var result = await _mediator.Send(new NumberGenerated(), stoppingToken);

                // Log the response from MediatR
                _logger.LogInformation($"Number saved to database: {result}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving number to database");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken); // Adjust delay as needed
        }
    }
}