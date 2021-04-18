using System;

namespace RateCalculationEngine.RateTypeChecker
{
    public class WeekendRateChecker: IRateTypeChecker
    {
        public bool IsRateApplicable(DateTime enterAt, DateTime exitAt)
        {
            switch (enterAt.DayOfWeek)
            {
                case DayOfWeek.Saturday when exitAt.DayOfWeek == DayOfWeek.Saturday && enterAt.Day == exitAt.Day:
                case DayOfWeek.Saturday when exitAt.DayOfWeek == DayOfWeek.Sunday && (exitAt - enterAt).Days == 1:
                case DayOfWeek.Sunday when exitAt.DayOfWeek == DayOfWeek.Sunday && enterAt.Day == exitAt.Day:
                    return true;
                default:
                    return false;
            }
        }
    }
}