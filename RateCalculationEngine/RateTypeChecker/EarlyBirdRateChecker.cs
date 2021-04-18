using System;

namespace RateCalculationEngine.RateTypeChecker
{
    public class EarlyBirdRateChecker: IRateTypeChecker
    {
        public bool IsRateApplicable(DateTime enterAt, DateTime exitAt)
        {
            if (enterAt.Day != exitAt.Day) return false;
            
            var enterFrom = new TimeSpan(6, 0, 0);
            var enterTill = new TimeSpan(9, 0, 0);

            var exitFrom = new TimeSpan(15, 30, 0);
            var exitTill = new TimeSpan(23, 30, 0);

            return enterAt.TimeOfDay > enterFrom && enterAt.TimeOfDay < enterTill && 
                   exitAt.TimeOfDay > exitFrom && exitAt.TimeOfDay < exitTill;
        }
    }
}