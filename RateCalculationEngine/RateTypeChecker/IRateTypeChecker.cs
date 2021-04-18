using System;

namespace RateCalculationEngine.RateTypeChecker
{
    public interface IRateTypeChecker
    {
        public bool IsRateApplicable(DateTime entry, DateTime exit);
    }
}