using System;
using System.Collections.Generic;
using System.Linq;
using RateCalculationEngine.Models;
using RateCalculationEngine.RateCalculator;

namespace RateCalculationEngine.Services
{
    public class ParkFeeCalculator: IParkFeeCalculator
    {
        private readonly IVariableRateCalculator _variableRateCalculator;
        private readonly Func<RateType, IFlatRateCalculator> _flatRateCalculatorFactory;

        public ParkFeeCalculator(Func<RateType, IFlatRateCalculator> factory, IVariableRateCalculator variableRateCalculator)
        {
            _flatRateCalculatorFactory = factory;
            _variableRateCalculator = variableRateCalculator;
        }
        
        public Rate CalculateParkingFee(DateTime enterAt, DateTime exitAt)
        {
            var candidateRates = new List<Rate>();
            IRateTypeService typeService = new RateTypeService();
            
            var rateTypes = typeService.DetermineRates(enterAt, exitAt);
            foreach (var rateType in rateTypes)
            {
                if (rateType == RateType.Standard)
                {
                    candidateRates.Add(_variableRateCalculator.CalculateRate(enterAt, exitAt));
                    break;
                }
                IFlatRateCalculator flatRateCalculator = _flatRateCalculatorFactory(rateType);
                candidateRates.Add(flatRateCalculator.CalculateRate());
            }
            
            return candidateRates.OrderBy(x => x.Price).ToList().First();
        }
    }
}