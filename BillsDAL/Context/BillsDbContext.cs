using BillsEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BillsDAL.Context
{
    public class BillsDbContext : DbContext
    {
        public BillsDbContext(DbContextOptions<BillsDbContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<BillHeader> BillHeaders { get; set; }
        public DbSet<BillDetails> BillDetails { get; set; }
        public DbSet<Stores> Stores { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesBillHeader> SalesBillHeaders { get; set; }
        public DbSet<SalesBillDetails> SalesBillDetails { get; set; }

    }
}
