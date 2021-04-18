using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using RateCalculationEngine.Models;
using RateCalculationEngine.RateCalculator;
using RateCalculationEngine.Services;
using Shouldly;
using TestStack.BDDfy;

namespace RateCalculationEngine.Test.Services
{
    [TestFixture]
    public class ParkingFeeCalculatorTests
    {
        private IRateTypeService _rateTypeService;
        private IVariableRateCalculator _variableRateCalculator;
        private Func<RateType, IFlatRateCalculator> _flatRateCalculatorFactory;
        
        private IParkingFeeCalculator _subject;

        private Rate _result;
        
        [SetUp]
        public void SetUp()
        {
            _rateTypeService = Substitute.For<IRateTypeService>();
            _variableRateCalculator = Substitute.For<IVariableRateCalculator>();
            _flatRateCalculatorFactory = Substitute.For<Func<RateType, IFlatRateCalculator>>();

            _subject = new ParkingFeeCalculator(_rateTypeService, _flatRateCalculatorFactory, _variableRateCalculator);
        }

        [Test]
        public void ItShouldReturnTheCheapestDealIfMultipleRatesAreApplicable()
        {
            this.Given(x => GivenBothEarlyBirdRateAndWeekendRateAreApplicable())
                .When(x => WhenCalculateParkingCost())
                .Then(x => ThenWeekendRateShouldBeReturned())
                .BDDfy();
        }

        private void GivenBothEarlyBirdRateAndWeekendRateAreApplicable()
        {
            _rateTypeService.DetermineApplicableRates(Arg.Any<DateTime>(), Arg.Any<DateTime>())
                .Returns(new List<RateType>{RateType.EarlyBird, RateType.Weekend});
            
            _flatRateCalculatorFactory(RateType.EarlyBird).CalculateRate().Returns(new Rate{Name = "Early Bird", Price = 13m});
            _flatRateCalculatorFactory(RateType.Weekend).CalculateRate().Returns(new Rate{Name = "Weekend Rate", Price = 10m});
        }

        private void WhenCalculateParkingCost()
        {
            _result = _subject.CalculateParkingFee(new DateTime(), new DateTime());
        }

        private void ThenWeekendRateShouldBeReturned()
        {
            _result.Name.ShouldBe("Weekend Rate");
            _result.Price.ShouldBe(10m);
        }
    }
}