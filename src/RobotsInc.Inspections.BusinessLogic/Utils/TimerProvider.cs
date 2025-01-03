using System;

namespace RobotsInc.Inspections.BusinessLogic.Utils
{
    public class TimerProvider : ITimerProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
