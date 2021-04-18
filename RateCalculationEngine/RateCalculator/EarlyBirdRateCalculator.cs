using RateCalculationEngine.Models;

namespace RateCalculationEngine.RateCalculator
{
    public class EarlyBirdRateCalculator: IFlatRateCalculator
    {
        public Rate CalculateRate()
        {
            return new Rate{Name = "Early Bird", Price = 13};
        }
    }
}