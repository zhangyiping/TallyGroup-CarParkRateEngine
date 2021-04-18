using System;
using RateCalculationEngine.Models;

namespace RateCalculationEngine.RateCalculator
{
    public class StandardRateCalculator: IVariableRateCalculator
    {
        private const decimal DayRate = 20;
        private const string RateName = "Standard";
        
        public Rate CalculateRate(DateTime entry, DateTime exit)
        {
            var daysOfParking = exit.Day - entry.Day;
            if (daysOfParking > 0)
            {
                return new Rate {Name = RateName, Price = daysOfParking * DayRate};
            }

            var timeOfParking = exit - entry;
            if (timeOfParking <= new TimeSpan(1, 0, 0))
            {
                return new Rate{Name = RateName, Price = 5};
            }
            if (new TimeSpan(1,0,0) < timeOfParking && timeOfParking <= new TimeSpan(2, 0, 0))
            {
                return new Rate{Name = RateName, Price = 10};
            }
            if (new TimeSpan(2,0,0) < timeOfParking && timeOfParking <= new TimeSpan(3, 0, 0))
            {
                return new Rate{Name = RateName, Price = 15};
            }

            return new Rate {Name = RateName, Price = DayRate};
        }
    }
}