using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RobotsInc.Inspections.API.I;
using RobotsInc.Inspections.API.I.Health;
using RobotsInc.Inspections.BusinessLogic.Health;

namespace RobotsInc.Inspections.Server
{
    [ApiController]
    [Route(Routes.Health)]
    public class HealthController
        : ControllerBase
    {
        private readonly IHealthManager _healthManager;
        private readonly ILogger<HealthController> _logger;
        public HealthController(
            IHealthManager healthManager,
            ILogger<HealthController> logger)
        {
            _healthManager = healthManager;
            _logger = logger;
        }

        [HttpGet("check-health")]
        public async Task<IActionResult> Health(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Requested healt check");
            HealthResult health =
                await _healthManager.CheCkHealthAsync(cancellationToken);
            return
                health.Status == HealthStatus.HEALTHY
                ? Ok(health)
                : StatusCode(StatusCodes.Status503ServiceUnavailable, health);
        }
    }
}
