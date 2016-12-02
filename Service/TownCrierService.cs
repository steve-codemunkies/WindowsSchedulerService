using System;
using System.Timers;
using Serilog;

namespace Service
{
    public class TownCrierService : ITestService
    {
        private readonly ILogger _logger;
        private readonly Timer _timer;

        public TownCrierService(ILogger logger)
        {
            _logger = logger;
            _timer = new Timer(1000) {AutoReset = true};
            _timer.Elapsed += (sender, args) => _logger.Information("timer fired, current time {0:G}", DateTime.UtcNow);
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