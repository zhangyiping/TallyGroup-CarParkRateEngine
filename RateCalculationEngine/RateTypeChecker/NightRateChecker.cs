using System;

namespace RateCalculationEngine.RateTypeChecker
{
    public class NightRateChecker: IRateTypeChecker
    {
        public bool IsRateApplicable(DateTime entry, DateTime exit)
        {
            if (entry.DayOfWeek == DayOfWeek.Saturday || entry.DayOfWeek == DayOfWeek.Sunday) return false;
            if ((exit - entry).Days != 1) return false;
            
            var enterFrom = new TimeSpan(18, 0, 0);
            var enterTill = new TimeSpan(0, 0, 0);
            
            var exitTill = new TimeSpan(8, 0, 0);

            return entry.TimeOfDay > enterFrom && entry.TimeOfDay < enterTill && exit.TimeOfDay < exitTill;
        }
    }
}