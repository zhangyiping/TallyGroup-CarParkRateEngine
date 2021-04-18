using System;
using System.Collections.Generic;
using System.Linq;
using RateCalculationEngine.Models;
using RateCalculationEngine.RateCalculator;
using RateCalculationEngine.RateTypeChecker;

namespace RateCalculationEngine.Services
{
    public class ParkFeeCalculator
    {
        private readonly IRateTypeService _rateTypeService;
        private readonly IVariableRateCalculator _variableRateCalculator;
        private readonly Func<RateType, IFlatRateCalculator> _flatRateCalculatorFactory;

        public ParkFeeCalculator(IRateTypeService rateTypeService, Func<RateType, IFlatRateCalculator> factory,
            IVariableRateCalculator variableRateCalculator)
        {
            _rateTypeService = rateTypeService;
            _flatRateCalculatorFactory = factory;
            _variableRateCalculator = variableRateCalculator;
        }
        
        public Rate CalculateParkingFee(DateTime enterAt, DateTime exitAt)
        {
            var candidateRates = new List<Rate>();

            var applicableRateTypes = _rateTypeService.DetermineApplicableRates(enterAt, exitAt);
            foreach (var rateType in applicableRateTypes)
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