using System;
using RateCalculationEngine.Models;

namespace RateCalculationEngine.Services
{
    public interface IParkingFeeCalculator
    {
        Rate CalculateParkingFee(DateTime enterAt, DateTime exitAt);
    }
}