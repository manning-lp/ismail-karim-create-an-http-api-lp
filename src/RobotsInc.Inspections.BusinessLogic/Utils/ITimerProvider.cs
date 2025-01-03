using System;

namespace RobotsInc.Inspections.BusinessLogic.Utils
{
    public interface ITimerProvider
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
