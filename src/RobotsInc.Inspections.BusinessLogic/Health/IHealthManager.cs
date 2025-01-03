using System.Threading;
using System.Threading.Tasks;
using RobotsInc.Inspections.API.I.Health;

namespace RobotsInc.Inspections.BusinessLogic.Health
{
    public interface IHealthManager
    {
        Task<HealthResult> CheCkHealthAsync(CancellationToken cancellationToken);
    }
}
