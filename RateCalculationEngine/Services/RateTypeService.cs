using System;
using System.Collections.Generic;
using System.Linq;
using RateCalculationEngine.Models;

namespace RateCalculationEngine.Services
{
    public class RateTypeService: IRateTypeService
    {
        public List<RateType> DetermineRates(DateTime entry, DateTime exit)
        {
            var candidateRateTypes = new List<RateType>();

            if (IsEarlyBirdRate(entry, exit))
            {
                candidateRateTypes.Add(RateType.EarlyBird);
            }
            if (IsNightRate(entry, exit))
            {
                candidateRateTypes.Add(RateType.Night);
            }
            if (IsWeekendRate(entry, exit))
            {
                candidateRateTypes.Add(RateType.Weekend);
            }

            if (!candidateRateTypes.Any())
            {
                candidateRateTypes.Add(RateType.Standard);
            }

            return candidateRateTypes;
        }

        private static bool IsEarlyBirdRate(DateTime entry, DateTime exit)
        {
            if (entry.Day != exit.Day) return false;
            
            var enterFrom = new TimeSpan(6, 0, 0);
            var enterTill = new TimeSpan(9, 0, 0);

            var exitFrom = new TimeSpan(15, 30, 0);
            var exitTill = new TimeSpan(23, 30, 0);

            return entry.TimeOfDay > enterFrom && entry.TimeOfDay < enterTill && 
                   exit.TimeOfDay > exitFrom && exit.TimeOfDay < exitTill;
        }

        private static bool IsNightRate(DateTime entry, DateTime exit)
        {
            if (entry.DayOfWeek == DayOfWeek.Saturday || entry.DayOfWeek == DayOfWeek.Sunday) return false;
            if ((exit - entry).Days != 1) return false;
            
            var enterFrom = new TimeSpan(18, 0, 0);
            var enterTill = new TimeSpan(0, 0, 0);
            
            var exitTill = new TimeSpan(8, 0, 0);

            return entry.TimeOfDay > enterFrom && entry.TimeOfDay < enterTill && exit.TimeOfDay < exitTill;
        }
        
        private static bool IsWeekendRate(DateTime entry, DateTime exit)
        {
            switch (entry.DayOfWeek)
            {
                case DayOfWeek.Saturday when exit.DayOfWeek == DayOfWeek.Saturday && entry.Day == exit.Day:
                case DayOfWeek.Saturday when exit.DayOfWeek == DayOfWeek.Sunday && (exit - entry).Days == 1:
                case DayOfWeek.Sunday when exit.DayOfWeek == DayOfWeek.Sunday && entry.Day == exit.Day:
                    return true;
                default:
                    return false;
            }
        }
    }
}