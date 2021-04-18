using System;
using Autofac;
using RateCalculationEngine.Models;
using RateCalculationEngine.RateCalculator;
using RateCalculationEngine.Services;

namespace RateCalculationEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = BuildContainer();
            var factory = container.Resolve<Func<RateType, IFlatRateCalculator>>();

            var enterAt = new DateTime(2021, 4, 16, 10, 0, 0);
            var exitAt = new DateTime(2021, 4, 16, 16, 0, 0);

            IParkFeeCalculator hello = new ParkFeeCalculator(factory, container.Resolve<IVariableRateCalculator>());
            var result = hello.CalculateParkingFee(enterAt, exitAt);
            Console.WriteLine(result.Name);

        }
        
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<RateTypeService>().AsImplementedInterfaces();
            builder.RegisterType<StandardRateCalculator>().AsImplementedInterfaces();
            
            builder.RegisterType<EarlyBirdRateCalculator>()
                .As<IFlatRateCalculator>()
                .Keyed<IFlatRateCalculator>(RateType.EarlyBird);
            
            builder.RegisterType<NightRateCalculator>()
                .As<IFlatRateCalculator>()
                .Keyed<IFlatRateCalculator>(RateType.Night);
            
            builder.RegisterType<WeekendRateCalculator>()
                .As<IFlatRateCalculator>()
                .Keyed<IFlatRateCalculator>(RateType.Weekend);
            
            builder.Register<Func<RateType, IFlatRateCalculator>>(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                return named => cc.ResolveKeyed<IFlatRateCalculator>(named);
            });
            return builder.Build();
        }
    }
}