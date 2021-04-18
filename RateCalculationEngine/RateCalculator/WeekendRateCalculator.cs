using RateCalculationEngine.Models;

namespace RateCalculationEngine.RateCalculator
{
    public class WeekendRateCalculator: IFlatRateCalculator
    {
        public Rate CalculateRate()
        {
            return new Rate {Name = "Weekend Rate", Price = 10};
        }
    }
}