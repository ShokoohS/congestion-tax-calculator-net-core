using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using congestion.calculator;

public class CongestionTaxCalculator
{
    /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total congestion tax for that day
         */

    private static readonly (TimeSpan StartTime, TimeSpan EndTime, int Amount)[] TaxTable = new[]
    {
        (new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 59), 8),
        (new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 59), 13),
        (new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 59), 18),
        (new TimeSpan(8, 0, 0), new TimeSpan(8, 29, 59), 13),
        (new TimeSpan(8, 30, 0), new TimeSpan(14, 59, 59), 8),
        (new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 59), 13),
        (new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 59), 18),
        (new TimeSpan(17, 0, 0), new TimeSpan(17, 59, 59), 13),
        (new TimeSpan(18, 0, 0), new TimeSpan(18, 29, 59), 8),
        (new TimeSpan(18, 30, 0), new TimeSpan(23, 59, 59), 0),
        (new TimeSpan(0, 0, 0), new TimeSpan(5, 59, 59), 0)
    };


    public int GetTax(Vehicle vehicle, DateTime[] initialDates)
    {
        var dates = new DateTime[initialDates.Length + 1];
        dates[0] = DateTime.MinValue;
        initialDates.CopyTo(dates, 1);
        dates = dates.OrderBy(x => x.Date).ToArray();
        DateTime intervalStart = dates[0];
        int totalFee = 0;


        for (int i = 1; i < dates.Length; i++)
        {
            if (dates[i].Date.Equals(dates[i - 1].Date) && dates[i].Hour == dates[i - 1].Hour && dates[i].Minute != dates[i - 1].Minute)
            {
                intervalStart = dates[i - 1];
            }
            int nextFee = GetTollFee(dates[i], vehicle);
            int tempFee = GetTollFee(intervalStart, vehicle);

            if ((dates[i] - intervalStart).TotalMinutes <= 60 && dates[i].Date.Equals(intervalStart.Date) && dates[i].Hour == intervalStart.Hour)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }

        if (totalFee > 60) totalFee = 60;
        return totalFee;

    }




    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorcycle.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        foreach (var entry in TaxTable)
        {
            if (date.TimeOfDay >= entry.StartTime && date.TimeOfDay <= entry.EndTime)
            {
                return entry.Amount;
            }
        }
        return 0;

    }

    private bool IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        if (year == 2013)
        {
            int[] tollFreeMonths = { 1, 3, 4, 5, 6, 7, 11, 12 };
            int[] tollFreeDays = { 1, 8, 9, 24, 25, 26, 28, 29, 30, 31 };

            if (tollFreeMonths.Contains(month) && tollFreeDays.Contains(day))
                return true;
        }

        return false;
    }


    private enum TollFreeVehicles
    {
        Motorcycle = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}