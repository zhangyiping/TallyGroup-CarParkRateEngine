using System;

namespace RateCalculationEngine.RateTypeChecker
{
    public interface IRateTypeChecker
    {
        public bool IsRateApplicable(DateTime enterAt, DateTime exitAt);
    }
}