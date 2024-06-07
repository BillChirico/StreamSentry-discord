using StreamSentry.Core.Bot;

namespace StreamSentry.Worker;

public class StreamSentryWorker(ILogger<StreamSentryWorker> logger, IBot bot) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker is starting bot at: {Time}", DateTimeOffset.Now);

        stoppingToken.Register(async () => await bot.Stop());

        await bot.Start();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await bot.Stop();

        logger.LogInformation("Worker has stopped bot at {Time}", DateTimeOffset.Now);
    }
}