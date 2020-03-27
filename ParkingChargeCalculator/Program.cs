using Microsoft.Extensions.DependencyInjection;
using System;

namespace ParkingChargeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ConsoleApplication>().Run(args);
        }
        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ConsoleApplication>();
            Random rnd = new Random();
            int randomNumber = rnd.Next(0, 200000);
            DateTime start = DateTime.Now.AddSeconds(0 - randomNumber);
            decimal shortStayRate = 1.1M;
            decimal longStayRate = 7.50M;
            services.AddTransient<IChargeCalculator, ChargeCalculator>(t => new ChargeCalculator(start, DateTime.Now, shortStayRate, longStayRate));
            return services;
        }
    }
}
