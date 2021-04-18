using System;
using RateCalculationEngine.Models;

namespace RateCalculationEngine.RateCalculator
{
    public interface IVariableRateCalculator
    {
        public Rate CalculateRate(DateTime entry, DateTime exit);
    }
}