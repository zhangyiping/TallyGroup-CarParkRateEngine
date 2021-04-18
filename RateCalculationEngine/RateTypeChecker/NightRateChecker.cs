using System;

namespace RateCalculationEngine.RateTypeChecker
{
    public class NightRateChecker: IRateTypeChecker
    {
        public bool IsRateApplicable(DateTime enterAt, DateTime exitAt)
        {
            if (enterAt.DayOfWeek == DayOfWeek.Saturday || enterAt.DayOfWeek == DayOfWeek.Sunday) return false;
            if ((exitAt - enterAt).Days != 1) return false;
            
            var enterFrom = new TimeSpan(18, 0, 0);
            var enterTill = new TimeSpan(0, 0, 0);
            
            var exitTill = new TimeSpan(8, 0, 0);

            return enterAt.TimeOfDay > enterFrom && enterAt.TimeOfDay < enterTill && exitAt.TimeOfDay < exitTill;
        }
    }
}