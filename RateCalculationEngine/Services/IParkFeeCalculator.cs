using System;
using RateCalculationEngine.Models;

namespace RateCalculationEngine.Services
{
    public interface IParkFeeCalculator
    {
        public Rate CalculateParkingFee(DateTime enterAt, DateTime exitAt);
    }
}