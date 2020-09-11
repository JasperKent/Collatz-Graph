using CollatzLibrary;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleCached
{
    class Program
    {
        static void Main()
        {
            try
            {
                Console.Write("Calculate up to? ");

                long upper = long.Parse(Console.ReadLine());

                CollatzCalculator calculator = new CollatzCalculator();

                // Key is a particular peak value. Value is the number of sequences which have that peak value.
                ConcurrentDictionary<ulong, ulong> peakFrequency = new ConcurrentDictionary<ulong, ulong>();

                SequenceItem longest = new SequenceItem(0);     // Item for head of sequence with longest count
                SequenceItem highest = new SequenceItem(0);     // Item for head of sequence with highest peak

                object locker = new object();

                Parallel.For(1, upper, seed =>
                {
                    var entry = calculator.GetEntry((ulong)seed);

                    peakFrequency.AddOrUpdate(entry.Peak, 1, (k, v) => v + 1); // Incremen count for this peak

                    lock (locker)
                    {
                        // Check and update longest
                        if (entry.Count > longest.Count)
                            longest = entry;

                        // Check and update highest peak. In the event of a tie, choose the shortest sequence
                        if (entry.Peak > highest.Peak || entry.Peak == highest.Peak && entry.Count < highest.Count)
                            highest = entry;
                    }
                });

                Display(longest, "Longest sequence");

                Console.WriteLine();

                Display(highest, "Highest peak");

                Console.WriteLine();

                var mostFrequent = peakFrequency.OrderByDescending(p => p.Value).Take(3);

                Console.WriteLine("Most common peaks (top 3):");

                foreach (var pair in mostFrequent)
                    Console.WriteLine($"   {pair.Key:N0}  ({pair.Value:N0} occurrences).");

                Console.WriteLine();

                Console.WriteLine($"{calculator.AllocationCount:N0} numbers calculated with a peak value of {calculator.PeakValue:N0}.");

                // Helper to display a full sequence
                void Display (SequenceItem item, string label)
                {
                    Console.WriteLine($"{label} with a seed up to {upper:N0} is for seed of {item.Current:N0} with {item.Count:N0} elements\n\tand with a peak value of {item.Peak:N0}.");

                    foreach (var entry in calculator.Sequence(item.Current))
                        Console.WriteLine($"{entry.Count:N0}: {entry.Current:N0} {(item.Peak == entry.Current ? "-- PEAK --" : "")}");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();
        }
    }
}
