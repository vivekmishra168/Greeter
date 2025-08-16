using Greeter.Services;
using log4net;
using Microsoft.Extensions.Hosting;

namespace Greeter
{
    public class Worker : BackgroundService
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Worker));
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private Timer? _timer;
        private readonly IGreetingService _greetingService;

        public Worker(IGreetingService greetingService)
        {
            _greetingService = greetingService ?? throw new ArgumentNullException(nameof(greetingService));
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _log.Info("StartAsync called.");
            // Start a timer to execute DoWork every 10 seconds.
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            if(!await _semaphore.WaitAsync(0))
            {
                _log.Warn("DoWork is already running, skipping this execution.");
                return;
            }
            _log.Info("DoWork called.");
            try
            {
                var message = await _greetingService.GreetAsync("World");
                _log.Info($"Greeting message: {message}");
            }
            catch (Exception ex)
            {
                _log.Error("An error occurred while greeting.", ex);
            }
            finally
            {
                _semaphore.Release();
            }
            _log.Info("DoWork completed.");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _log.Info("StopAsync called.");
            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();
            _semaphore.Dispose();
            return Task.CompletedTask;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
