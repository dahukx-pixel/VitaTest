using Microsoft.EntityFrameworkCore;
using VitaTest.Domain.Models;

namespace VitaTest.Infrastructure.Database
{
    public class VitaDatabase : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DataBaseVersion> DataBaseVersion { get; set; }

        public VitaDatabase(DbContextOptions<VitaDatabase> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                        .HasOne(p => p.Order)
                        .WithMany(o => o.Payments)
                        .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<Payment>()
                        .HasOne(p => p.Income)
                        .WithMany(i => i.Payments)
                        .HasForeignKey(p => p.IncomeId);
        }
    }
}
