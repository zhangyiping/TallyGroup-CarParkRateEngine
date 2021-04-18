using RateCalculationEngine.Models;

namespace RateCalculationEngine.RateCalculator
{
    public interface IFlatRateCalculator
    {
        public Rate CalculateRate();
    }
}