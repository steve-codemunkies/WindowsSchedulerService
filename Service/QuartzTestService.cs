using System.Collections.Generic;
using Quartz;
using Serilog;

namespace Service
{
    public class QuartzTestService : ITestService
    {
        private readonly IScheduler _jobScheduler;
        private readonly ILogger _logger;

        public QuartzTestService(IScheduler jobScheduler, ILogger logger)
        {
            _jobScheduler = jobScheduler;
            _logger = logger;
        }

        public void Start()
        {
            _jobScheduler.Start();
            _logger.Information("Job scheduler started");
        }

        public void Stop()
        {
            _jobScheduler.Shutdown(true);
            _logger.Information("Job scheduler stopped");
        }
    }
}