using System;
using System.Collections.Generic;
using System.Text;

namespace CollatzLibrary
{
    public struct SequenceItem
    {
        public SequenceItem(ulong current):this()
        {
            Current = current;
        }

        public SequenceItem(ulong current, uint count, ulong peak) 
        {
            Current = current;
            Count = count;
            Peak = peak;
        }

        public ulong Current { get; set; }  // Current value in the sequence. Could probably be omitted as it's the  key in the dictionary, but makes things easier to understand
        public uint Count { get; set; }     // Number of elemenets in sequence beginning with Current
        public ulong Peak { get; set; }     // Highest value in sequence beginning with Current

        public ulong Next => Current % 2 == 0 ? Current / 2 : Current * 3 + 1;  // Next value in sequence. Could a;ternatively be calculated once in constructor and stored, which would be less CPU but more memory
    }
}
