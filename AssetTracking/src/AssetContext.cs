using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetTracking.src
{

    //DATABASE CONNECTION CLASS
    class AssetContext : DbContext
    {
        public DbSet<Computer> Computers { get; set; }
        public DbSet<Phone> Phones { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
              "Server = (localdb)\\mssqllocaldb; Database = AssetDatabase;");
        }
    }
}
