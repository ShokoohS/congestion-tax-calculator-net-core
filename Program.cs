
using System;

namespace congestion.calculator
{
    public class Program
    {

        static void Main(string[] args)
        {
            CongestionTaxCalculator calculator = new CongestionTaxCalculator();

            DateTime[] passages = {
                new DateTime(2013,1, 14, 21, 00, 00),
                new DateTime(2013,1, 15, 21, 00, 00),
                new DateTime(2013,2, 7, 6, 23, 27),
                new DateTime(2013,2, 7, 15, 27, 00),
                new DateTime(2013,2, 8, 6, 27, 00),
                new DateTime(2013,2, 8, 6, 20, 27),
                new DateTime(2013,2, 8, 14, 35, 00),
                new DateTime(2013,2, 8, 15, 29, 00),
                new DateTime(2013,2, 8, 15, 47, 00),
                new DateTime(2013,2, 8, 16, 01, 00),
                new DateTime(2013,2, 8, 16, 48, 00),
                new DateTime(2013,2, 8, 17, 49, 00),
                new DateTime(2013,2, 8, 18, 29, 00),
                new DateTime(2013,2, 8, 18, 35, 00),
                new DateTime(2013,3, 26, 14, 25, 00),
                new DateTime(2013,3, 28, 14, 7, 27)

            };

            var car = new Car();
            int totalTax = calculator.GetTax(car, passages);
            Console.WriteLine($"Total Congestion Tax: {totalTax} SEK");
        }
    }
}
