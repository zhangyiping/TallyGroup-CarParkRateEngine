using System;
using NUnit.Framework;
using RateCalculationEngine.RateTypeChecker;
using Shouldly;
using TestStack.BDDfy;

namespace RateCalculationEngine.Test.RateTypeChecker
{
    [TestFixture]
    public class EarlyBirdRateCheckerTests
    {
        private DateTime _enterAt;
        private DateTime _exitAt;
        
        private IRateTypeChecker _subject;

        private bool _result;

        [SetUp]
        public void SetUp()
        {
            _subject = new EarlyBirdRateChecker();
        }

        [Test]
        public void ItShouldReturnTrueIfEntryAndExitConditionsAreSatisfied()
        {
            this.Given(x => GivenCustomerEntersAt(new DateTime(2021, 4, 20, 7,0,0)))
                .And(x => GivenCustomerExitAt(new DateTime(2021, 4,20,16,0,0)))
                .When(x => WhenDetermineIfRateIsApplicable())
                .Then(x => ThenTrueShouldBeReturned())
                .BDDfy();
        }
        
        [Test]
        public void ItShouldReturnFalseIfEntryAndExitConditionsAreNotSatisfied()
        {
            this.Given(x => GivenCustomerEntersAt(new DateTime(2021, 4, 20, 10,0,0)))
                .And(x => GivenCustomerExitAt(new DateTime(2021, 4,20,16,0,0)))
                .When(x => WhenDetermineIfRateIsApplicable())
                .Then(x => ThenTrueShouldBeFalse())
                .BDDfy();
        }
        
        [Test]
        public void ItShouldReturnFalseIfEntryAndExitAreNotOnTheSameDay()
        {
            this.Given(x => GivenCustomerEntersAt(new DateTime(2021, 4, 20, 7,0,0)))
                .And(x => GivenCustomerExitAt(new DateTime(2021, 4,21,16,0,0)))
                .When(x => WhenDetermineIfRateIsApplicable())
                .Then(x => ThenTrueShouldBeFalse())
                .BDDfy();
        }

        private void GivenCustomerEntersAt(DateTime enterAt)
        {
            _enterAt = enterAt;
        }

        private void GivenCustomerExitAt(DateTime exitAt)
        {
            _exitAt = exitAt;
        }

        private void WhenDetermineIfRateIsApplicable()
        {
            _result = _subject.IsRateApplicable(_enterAt, _exitAt);
        }

        private void ThenTrueShouldBeReturned()
        {
            _result.ShouldBe(true);
        }
        
        private void ThenTrueShouldBeFalse()
        {
            _result.ShouldBe(false);
        }
    }
}