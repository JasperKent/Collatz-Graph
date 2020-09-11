using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CollatzLibrary
{
    public class CollatzCalculator
    {
        private readonly ConcurrentDictionary<ulong, SequenceItem> _sequence = new ConcurrentDictionary<ulong, SequenceItem>();

        public SequenceItem GetEntry (ulong seed)
        {
            if (seed <= 1)
                return new SequenceItem(seed) { Peak = 1, Count = 1 };
            else
            {
                var entry = _sequence.GetOrAdd(seed, x => new SequenceItem(seed));

                if (entry.Count == 0)
                {
                    var nextEntry = GetEntry(entry.Next);

                    var newEntry = new SequenceItem(entry.Current, nextEntry.Count + 1, Math.Max(entry.Current, nextEntry.Peak));

                    _sequence.TryUpdate(seed, newEntry, entry);

                    return newEntry;
                }
                else
                    return entry;
            }
        }

        public IEnumerable<SequenceItem> Sequence (ulong seed)
        {
            GetEntry(seed);

            uint count = 1;

            yield return new SequenceItem(seed) { Count = 1 };

            while (seed > 1)
            {
                var item = _sequence[seed];

                seed = item.Next;

                yield return new SequenceItem(seed) { Count = ++count };
            }

        }

        public int AllocationCount => _sequence.Count;
        public ulong PeakValue => _sequence.Max(kv => kv.Key);
    }
}
