using System;

namespace RateCalculationEngine.RateTypeChecker
{
    public class WeekendRateChecker: IRateTypeChecker
    {
        public bool IsRateApplicable(DateTime entry, DateTime exit)
        {
            switch (entry.DayOfWeek)
            {
                case DayOfWeek.Saturday when exit.DayOfWeek == DayOfWeek.Saturday && entry.Day == exit.Day:
                case DayOfWeek.Saturday when exit.DayOfWeek == DayOfWeek.Sunday && (exit - entry).Days == 1:
                case DayOfWeek.Sunday when exit.DayOfWeek == DayOfWeek.Sunday && entry.Day == exit.Day:
                    return true;
                default:
                    return false;
            }
        }
    }
}