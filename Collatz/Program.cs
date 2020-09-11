using CollatzCached;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Collatz
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.Write("Calculate up to? ");
                
                int upper = int.Parse(Console.ReadLine());

                CollatzCalculator calculator = new CollatzCalculator(0X7FEFFFFF);

                calculator.Generate();

                (int count, int seed) longest = (0, 0);

                for (int seed = 1; seed < upper; ++seed)
                {
                    try
                    {
                        int count = calculator.Sequence(seed).Count();

                        if (count > longest.count)
                            longest = (count, seed);
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                foreach (var (count, value) in calculator.Sequence(longest.seed))
                    Console.WriteLine($"{count}: {value}");

                Console.WriteLine($"Longest sequence with a seed up to {upper} is for seed of {longest.seed} with {longest.count} elements.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
