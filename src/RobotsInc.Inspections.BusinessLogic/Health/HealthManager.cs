using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RobotsInc.Inspections.API.I.Health;
using RobotsInc.Inspections.BusinessLogic.Utils;

namespace RobotsInc.Inspections.BusinessLogic.Health
{
    public class HealthManager : IHealthManager
    {
        private readonly ITimerProvider _timerProvider;
        private readonly IOfficeHoursManager _officeHoursManager;
        private readonly ILogger<HealthManager> _logger;
        public HealthManager(
            ITimerProvider timerProvider,
            IOfficeHoursManager officeHoursManager,
            ILogger<HealthManager> logger)
        {
            _timerProvider = timerProvider ?? throw new ArgumentNullException(nameof(timerProvider));
            _officeHoursManager = officeHoursManager ?? throw new ArgumentNullException(nameof(timerProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<HealthResult> CheCkHealthAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting healt check");
            DateTime now = _timerProvider.Now;
            bool open = _officeHoursManager.IsWithinOfficeHours(now);
            HealthResult health =
                open ? new()
                {
                    Status = HealthStatus.HEALTHY,
                    Message = "Service up & running"
                }
                : new()
                {
                    Status = HealthStatus.CLOSED,
                    Message = "Outside of office hours: business closed"
                };
            _logger.LogDebug("Health check finished.");
            return Task.FromResult(health);
        }
    }
}
