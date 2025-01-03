using System;

namespace RobotsInc.Inspections.BusinessLogic.Health
{
    public interface IOfficeHoursManager
    {
        bool IsWithinOfficeHours(DateTime dateTime);
    }
}
