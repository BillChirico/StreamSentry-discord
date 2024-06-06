namespace StreamSentry.Worker;

public class Worker(ILogger<Worker> logger) : BackgroundService
{
    private readonly ILogger<Worker> _logger = logger;
    private readonly IBot _bot;

    public Worker(ILogger<Worker> logger, IBot bot)
    {
        _logger = logger;
        _bot = bot;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker is starting bot at: {time}", DateTimeOffset.Now);
        
        stoppingToken.Register(async () => await _bot.Stop());
        await _bot.Start();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _bot.Stop();

        _logger.LogInformation("Worker has stopped bot at {time}", DateTimeOffset.Now);
    }
}