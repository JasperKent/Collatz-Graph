using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace CollatzWithDB
{
    public class CollatzCalculator : IDisposable
    {
        private CollatzContext _db;

        public void Dispose() => _db?.Dispose();

        public CollatzCalculator(string connectionString)
        {
            _db = new CollatzContext(connectionString);
            _db.Database.EnsureCreated();
        }

        public BigInteger GetCount(BigInteger seed)
        {
            if (seed <= 1)
                return 1;
            else
            {
                var entry = _db.SequenceItems.Find(seed.ToString());

                if (entry != null)
                    return BigInteger.Parse(entry.Count);
                else
                {
                    BigInteger next = seed % 2 == 0 ? seed / 2 : seed * 3 + 1;

                    BigInteger count = GetCount(next) + 1;

                    var item = new SequenceItem { Value = seed.ToString(), NextValue = next.ToString(), Count = count.ToString() };

                    _db.SequenceItems.Add(item);

                    return count;
                }
            }
        }

        public IEnumerable<(BigInteger value, BigInteger count)> Sequence(BigInteger seed)
        {
            GetCount(seed);

            BigInteger count = 1;

            yield return (seed, count);

            while (seed > 1)
            {
                var item = _db.SequenceItems.Find(seed.ToString());

                seed = BigInteger.Parse(item.NextValue);

                yield return (seed, ++count);
            }
        }

        public void SaveChanges()
        {
            _db?.SaveChanges();
        }
    }
}
