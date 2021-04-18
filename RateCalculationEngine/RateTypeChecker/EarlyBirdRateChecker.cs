using System;

namespace RateCalculationEngine.RateTypeChecker
{
    public class EarlyBirdRateChecker: IRateTypeChecker
    {
        public bool IsRateApplicable(DateTime entry, DateTime exit)
        {
            if (entry.Day != exit.Day) return false;
            
            var enterFrom = new TimeSpan(6, 0, 0);
            var enterTill = new TimeSpan(9, 0, 0);

            var exitFrom = new TimeSpan(15, 30, 0);
            var exitTill = new TimeSpan(23, 30, 0);

            return entry.TimeOfDay > enterFrom && entry.TimeOfDay < enterTill && 
                   exit.TimeOfDay > exitFrom && exit.TimeOfDay < exitTill;
        }
    }
}