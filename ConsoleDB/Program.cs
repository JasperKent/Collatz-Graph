using CollatzWithDB;
using System;
using System.Configuration;
using System.Numerics;

namespace ConsoleDB
{
    class Program
    {
        static void Main()
        {
            using var calculator = new CollatzCalculator(ConfigurationManager.ConnectionStrings["Collatz"].ConnectionString);

            Console.Write("Calculate up to? ");

            var upper = BigInteger.Parse(Console.ReadLine());

            (BigInteger seed, BigInteger count) longest = (0, 0);

            //Parallel.For(0, upper, seed =>
            for (BigInteger seed = 0; seed < upper; ++seed)
            {
                var count = calculator.GetCount(seed);

                if (count > longest.count)
                    longest = (seed, count);

                calculator.SaveChanges();

            }//);

            foreach (var (value, count) in calculator.Sequence(longest.seed))
                Console.WriteLine($"{count:N0}: {value:N0}");

            Console.WriteLine($"Longest sequence with a seed up to {upper:N0} is for seed of {longest.seed:N0} with {longest.count:N0} elements.");
        //    Console.WriteLine($"{calculator.AllocationCount:N0} numbers calculated with a peak value of {calculator.PeakValue:N0}.");
        }
    }
}
