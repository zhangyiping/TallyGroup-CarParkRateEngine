using System;
using System.Collections.Generic;
using System.Linq;
using RateCalculationEngine.Models;
using RateCalculationEngine.RateTypeChecker;

namespace RateCalculationEngine.Services
{
    public class RateTypeService: IRateTypeService
    {
        private readonly Func<RateType, IRateTypeChecker> _rateTypeFactory;
        public RateTypeService(Func<RateType, IRateTypeChecker> rateTypeFactory)
        {
            _rateTypeFactory = rateTypeFactory;
        }
        
        public List<RateType> DetermineApplicableRates(DateTime enterAt, DateTime exitAt)
        {
            var candidateRateTypes = new List<RateType>();

            var allRateTypes = (RateType[]) Enum.GetValues(typeof(RateType));
            foreach (var rateType in allRateTypes)
            {
                if (rateType == RateType.Standard)
                {
                    continue;
                }

                IRateTypeChecker rateChecker = _rateTypeFactory(rateType);
                var isCurrentRateApplicable = rateChecker.IsRateApplicable(enterAt, exitAt);
                if (isCurrentRateApplicable)
                {
                    candidateRateTypes.Add(rateType);
                }
            }

            if (!IsFlatRateApplicable(candidateRateTypes))
            {
                candidateRateTypes.Add(RateType.Standard);
            }
            return candidateRateTypes;
        }

        private static bool IsFlatRateApplicable(List<RateType> rateTypes)
        {
            return rateTypes.Any();
        }
    }
}