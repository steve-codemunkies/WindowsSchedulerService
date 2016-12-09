using System;
using System.Timers;
using Serilog;

namespace Service
{
    public class TestService : ITestService
    {
        private readonly ILogger _logger;
        private readonly Timer _timer;

        public TestService(ILogger logger)
        {
            _logger = logger;
            _timer = new Timer(1000) {AutoReset = true};
            _timer.Elapsed += (sender, args) => _logger.Information($"Timer fired {DateTime.UtcNow:G}");
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}