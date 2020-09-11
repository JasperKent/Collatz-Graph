using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace CollatzWithDB
{
    public class SequenceItem
    {
        [Key]
        public string Value { get; set; }

        public string NextValue { get; set; }
        public string Count { get; set; }

        //[ForeignKey(nameof(NextValue))]
        //public SequenceItem NextItem { get; set; }
    }
}
