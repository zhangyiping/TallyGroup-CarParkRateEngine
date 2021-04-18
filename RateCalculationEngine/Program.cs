using System;
using Autofac;
using RateCalculationEngine.Models;
using RateCalculationEngine.RateCalculator;
using RateCalculationEngine.RateTypeChecker;
using RateCalculationEngine.Services;

namespace RateCalculationEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = BuildContainer();
            var flatRateCalculatorFactory = container.Resolve<Func<RateType, IFlatRateCalculator>>();
            IParkingFeeCalculator parkingFeeCalculator = new ParkingFeeCalculator(container.Resolve<IRateTypeService>(), 
                flatRateCalculatorFactory, container.Resolve<IVariableRateCalculator>());
            
            var enterAt = new DateTime(2021, 4, 16, 10, 0, 0);
            var exitAt = new DateTime(2021, 4, 16, 11, 30, 0);
            var result = parkingFeeCalculator.CalculateParkingFee(enterAt, exitAt);
            Console.WriteLine(result.Name);
            Console.WriteLine(result.Price);
        }
        
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<RateTypeService>().AsImplementedInterfaces();
            builder.RegisterType<StandardRateCalculator>().AsImplementedInterfaces();
            
            RegisterRateTypeChecker(builder);
            RegisterFlatRateCalculator(builder);
            return builder.Build();
        }

        private static void RegisterRateTypeChecker(ContainerBuilder builder)
        {
            builder.RegisterType<EarlyBirdRateChecker>()
                .As<IRateTypeChecker>()
                .Keyed<IRateTypeChecker>(RateType.EarlyBird);

            builder.RegisterType<NightRateChecker>()
                .As<IRateTypeChecker>()
                .Keyed<IRateTypeChecker>(RateType.Night);

            builder.RegisterType<WeekendRateChecker>()
                .As<IRateTypeChecker>()
                .Keyed<IRateTypeChecker>(RateType.Weekend);

            builder.Register<Func<RateType, IRateTypeChecker>>(c =>
            {
                var cc = c.Resolve<IComponentContext>();
                return named => cc.ResolveKeyed<IRateTypeChecker>(named);
            });
        }

        private static void RegisterFlatRateCalculator(ContainerBuilder builder)
        {
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
        }
    }
}