using System;
using RateCalculationEngine.Models;

namespace RateCalculationEngine.RateCalculator
{
    public interface IVariableRateCalculator
    {
        public Rate CalculateRate(DateTime enterAt, DateTime exitAt);
    }
}