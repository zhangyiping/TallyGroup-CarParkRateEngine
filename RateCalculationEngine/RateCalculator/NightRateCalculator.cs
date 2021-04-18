using RateCalculationEngine.Models;

namespace RateCalculationEngine.RateCalculator
{
    public class NightRateCalculator: IFlatRateCalculator
    {
        public Rate CalculateRate()
        {
            return new Rate {Name = "Night Rate", Price = 6.5m};
        }
    }
}