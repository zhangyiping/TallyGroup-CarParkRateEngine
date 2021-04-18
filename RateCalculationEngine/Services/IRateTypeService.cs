using System;
using System.Collections.Generic;
using RateCalculationEngine.Models;

namespace RateCalculationEngine.Services
{
    public interface IRateTypeService
    {
        List<RateType> DetermineApplicableRates(DateTime enterAt, DateTime exitAt);
    }
}