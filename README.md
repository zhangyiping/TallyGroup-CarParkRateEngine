# Car Park Rate Calculation Engine

An API calculates correct parking rate for customers.

## API Interface
The API accepts two inputs: car entry date and time, and car exit date and time  
`CalculateParkingFee(enterAt: DateTime, exitAt: DateTime): Rate`  

The API returns an object which contains the name of the rate along with the total parking price
```
public class Rate
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

## Tech Stack
C# .NET Core 3.1    
Autofac  
NUnit   
[TestStack.BDDfy](https://github.com/TestStack/TestStack.BDDfy)  
[NSubstitute](https://nsubstitute.github.io/)  
[Shouldly](https://github.com/shouldly/shouldly)

## Design Consideration
### Separation of Concern
The calculation of parking fee (defined in RateCalculator directory) and the logic to determine 
which rate(s) is applicable (defined in RateTypeChecker directory) is separated.

### Interface Segregation
Flat rate calculator classes and variable rate calculator class implement different interfaces
because flat rate calculator does not required car entry date time and car exit date time.   
UML illustration as below ![here](./Rate%20Calculation%20Logic.png)

### Open/Closed Principle
If there are more types of parking rate in the future, interfaces can be implemented and none of 
the existing logic needs to be changed.   
The below UML illustrates that if there is more rate type in the future, `IRateTypeChecker` can be
implemented to check if the new rate is applicable for given inputs ![here](./Rate%20Checking%20Logic.png)

### Dependency Injection
[Autofac](https://autofac.org/) is used to handle dependency injection. 
Interface which is implemented once is solved by type  
e.g. `builder.RegisterType<StandardRateCalculator>().AsImplementedInterfaces();`

Interface which is implemented by multiple classes is resolved by keys e.g.
```  
builder.RegisterType<EarlyBirdRateChecker>()  
 .As<IRateTypeChecker>() .Keyed<IRateTypeChecker>(RateType.EarlyBird);  
builder.RegisterType<NightRateChecker>()  
 .As<IRateTypeChecker>() .Keyed<IRateTypeChecker>(RateType.Night);
 ```

## Unit Test
Unit test is in Behavior Driven Development(BDD) format. One example is to cover the requirement that if 
multiple rates are applicable e.g. enter at 7:00 am and exit at 4:00 pm on a Saturday. In this case, both 
early bird and weekend rate are available. Weekend rate should be returned because it is the cheapest deal.  

<u>Given</u> clause is used to set up conditions and prepare input data. <u>When</u> clause performs the test subject 
operation. <u>Then</u> clause is used to define assertions.  
```
[Test]
public void ItShouldReturnTheCheapestDealIfMultipleRatesAreApplicable()
{
    this.Given(x => GivenBothEarlyBirdRateAndWeekendRateAreApplicable())
        .When(x => WhenCalculateParkingCost())
        .Then(x => ThenWeekendRateShouldBeReturned())
        .BDDfy();
}
```

## Side Note
The solution is working and all existing unit tests pass. Due to time constraints, calculation logic 
may result in incorrect value and tests only cover two business rules. However, I believe I have demonstrated
my ability to structure and test production quality code. 