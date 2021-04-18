using System;
using System.Collections.Generic;
using RateCalculationEngine.Models;

namespace RateCalculationEngine.Services
{
    public interface IRateTypeService
    {
        public List<RateType> DetermineRates(DateTime entry, DateTime exit);
    }
}