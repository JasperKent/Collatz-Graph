using System;
using System.Collections.Generic;

namespace CollatzCSharp
{
    class Program
    {
        public static IEnumerable<(uint count, uint value)> Collatz(uint seed)
        {
            uint count = 1;

            yield return (count, seed);

            while (seed > 1)
            {
                if (seed % 2 == 0)
                    seed /= 2;
                else
                    seed = 3 * seed + 1;

                yield return (++count, seed);
            }
        }

        static void Main()
        {
            foreach (var (count, value) in Collatz(7))
            {
                Console.WriteLine($"{count}: {value}.");
            }

            Console.WriteLine();

            foreach (var (count, value) in Collatz(6))
            {
                Console.WriteLine($"{count}: {value}.");
            }

            Console.WriteLine();

            foreach (var (count, value) in Collatz(5))
            {
                Console.WriteLine($"{count}: {value}.");
            }

            Console.WriteLine();

            foreach (var (count, value) in Collatz(4))
            {
                Console.WriteLine($"{count}: {value}.");
            }


            Console.WriteLine();

            foreach (var (count, value) in Collatz(3))
            {
                Console.WriteLine($"{count}: {value}.");
            }
        }
    }
}
