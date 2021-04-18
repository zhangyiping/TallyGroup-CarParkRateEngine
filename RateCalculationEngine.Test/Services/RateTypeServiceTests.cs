using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using RateCalculationEngine.Models;
using RateCalculationEngine.RateTypeChecker;
using RateCalculationEngine.Services;
using Shouldly;
using TestStack.BDDfy;

namespace RateCalculationEngine.Test.Services
{
    [TestFixture]
    public class RateTypeServiceTests
    {
        private Func<RateType, IRateTypeChecker> _rateTypeFactory;
        
        private IRateTypeService _subject;

        private List<RateType> _result;

        [SetUp]
        public void SetUp()
        {
            _rateTypeFactory = Substitute.For<Func<RateType, IRateTypeChecker>>();
            _subject = new RateTypeService(_rateTypeFactory);
            _result = new List<RateType>();
        }

        [Test]
        public void ItShouldReturnStandardRateIfNoFlatRateIsApplicable()
        {
            this.Given(x => GivenEarlyBirdRateIsNotApplicable())
                .And(x => GivenNightRateIsNotApplicable())
                .And(x => GivenWeekendRateIsNotApplicable())
                .When(x => WhenDetermineWhichRateIsApplicable())
                .Then(x => ThenOnlyStandardRateShouldBeApplicable())
                .BDDfy();
        }
        
        [Test]
        public void ItShouldNotReturnStandardRateIfOneOrMoreFlatRateIsApplicable()
        {
            this.Given(x => GivenEarlyBirdRateIsApplicable())
                .And(x => GivenNightRateIsNotApplicable())
                .And(x => GivenWeekendRateIsNotApplicable())
                .When(x => WhenDetermineWhichRateIsApplicable())
                .Then(x => ThenEarlyBirdRateShouldBeAvailable())
                .And(x => ThenStandardRateShouldNotBeAvailable())
                .BDDfy();
        }
        
        [Test]
        public void ItShouldReturnAllApplicableRates()
        {
            this.Given(x => GivenEarlyBirdRateIsApplicable())
                .And(x => GivenNightRateIsApplicable())
                .And(x => GivenWeekendRateIsNotApplicable())
                .When(x => WhenDetermineWhichRateIsApplicable())
                .Then(x => ThenEarlyBirdRateShouldBeAvailable())
                .And(x => ThenNightRateShouldBeAvailable())
                .BDDfy();
        }

        private void GivenNightRateIsApplicable()
        {
            _rateTypeFactory(RateType.Night).IsRateApplicable(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(true);
        }

        private void ThenNightRateShouldBeAvailable()
        {
            _result.ShouldContain(x => x == RateType.Night);
        }

        private void GivenEarlyBirdRateIsApplicable()
        {
            _rateTypeFactory(RateType.EarlyBird).IsRateApplicable(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(true);
        }

        private void ThenEarlyBirdRateShouldBeAvailable()
        {
            _result.ShouldContain(x => x == RateType.EarlyBird);
        }

        private void ThenStandardRateShouldNotBeAvailable()
        {
            _result.ShouldNotContain(x => x == RateType.Standard);
        }

        private void GivenNightRateIsNotApplicable()
        {
            _rateTypeFactory(RateType.Night).IsRateApplicable(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(false);
        }

        private void GivenWeekendRateIsNotApplicable()
        {
            _rateTypeFactory(RateType.Weekend).IsRateApplicable(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(false);
        }

        private void GivenEarlyBirdRateIsNotApplicable()
        {
            _rateTypeFactory(RateType.EarlyBird).IsRateApplicable(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(false);
        }

        private void WhenDetermineWhichRateIsApplicable()
        {
            _result = _subject.DetermineApplicableRates(new DateTime(), new DateTime());
        }

        private void ThenOnlyStandardRateShouldBeApplicable()
        {
            _result.Count.ShouldBe(1);
            _result.ShouldContain(x => x == RateType.Standard);
        }
    }
}