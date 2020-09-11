using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollatzWithDB
{
    public class CollatzContext : DbContext
    {
        private readonly string _connectionString;

        public CollatzContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<SequenceItem> SequenceItems { get; set; }
    }
}
