using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingChargeCalculator
{
    public class ChargeCalculator : IChargeCalculator
    {
        private readonly DateTime start;
        private readonly DateTime end;
        private readonly decimal shortStayRate;
        private readonly decimal longStayRate;
        private bool isLongStay;
        const DayOfWeek dayOfWeekFreePeriod1 = DayOfWeek.Saturday;
        const DayOfWeek dayOfWeekFreePeriod2 = DayOfWeek.Sunday;
        const int weekdayChargePeriodStart = 8;
        const int weekdayChargePeriodEnd = 18;

        public ChargeCalculator(DateTime start, DateTime end, decimal shortStayRate, decimal longStayRate)
        {
            this.start = start;
            this.end = end;
            this.shortStayRate = shortStayRate;
            this.longStayRate = longStayRate;
            // random bool
            Random rng = new Random();
            this.isLongStay = rng.Next(0, 2) > 0;
        }
        public void Action()
        {
            Console.WriteLine("Start: {0}", start);
            Console.WriteLine("End: {0}", end);
            Console.WriteLine(GetPrice(start, end, shortStayRate, longStayRate));
        }

        private string GetPrice(DateTime start, DateTime end, decimal shortStayRate, decimal longStayRate)
        {
            decimal stayPrice;
            TimeSpan elapsed = end - start;
            if (isLongStay)
                stayPrice = GetLongStayPrice(elapsed, longStayRate);
            else
                stayPrice = GetShortStayPrice(start, end, shortStayRate);
            return (stayPrice).ToString();
        }

        private bool IsNotChargeable(DateTime start, DateTime end)
        {
            if (isLongStay)
                return false;
            if (AnyWithinChargablePeriod(start, end))
                return false;
            return true;
        }

        private bool AnyWithinChargablePeriod(DateTime start, DateTime end)
        {
            if (start.DayOfWeek != dayOfWeekFreePeriod1 || start.DayOfWeek != dayOfWeekFreePeriod2)
                return true;
            if (end.DayOfWeek != dayOfWeekFreePeriod1 || end.DayOfWeek != dayOfWeekFreePeriod2)
                return true;
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek == dayOfWeekFreePeriod1 || date.DayOfWeek == dayOfWeekFreePeriod2)
                    return true;
            }
            for (DateTime date = start; date <= end; date = date.AddHours(1))
            {
                if (date.Hour >=  weekdayChargePeriodStart || date.Hour == weekdayChargePeriodEnd)
                    return true;
            }
            return false;
        }

        private decimal GetLongStayPrice(TimeSpan elapsed, decimal longStayRate)
        {
            int daysElapsed = elapsed.Hours;
            return daysElapsed * longStayRate;
        }

        private decimal GetShortStayPrice(DateTime start, DateTime end, decimal shortStayRate)
        {
            if (IsNotChargeable(start, end))
                return 0;
            int chargeableHours = GetChargeableHoursShortStay(start, end);
            return chargeableHours * shortStayRate;
        }

        private int GetChargeableHoursShortStay(DateTime start, DateTime end)
        {
            int totalChargeableHours = 0;
            for (DateTime date = start; date <= end; date = date.AddHours(1))
            {
                if (date.DayOfWeek == dayOfWeekFreePeriod1 || date.DayOfWeek == dayOfWeekFreePeriod2)
                    continue;
                if (date.Hour >= weekdayChargePeriodStart || date.Hour == weekdayChargePeriodEnd)
                    totalChargeableHours++;
            }
            return 10;
        }
    }
}
