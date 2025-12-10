using System;
using FlightStatistics.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlightStatistics.Server
{
    public partial class AirportContext : DbContext
    {
        public AirportContext(DbContextOptions<AirportContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<AirportMonthlyData> AirportMonthlyData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airport>(b =>
            {
                b.Property<string>("AirportCode");
                b.HasKey("AirportCode");
                b.HasAnnotation("Relational:TableName", "Airports");
            });

            modelBuilder.Entity<AirportMonthlyData>(b =>
            {
                b.Property<int>("SNo");
                b.HasKey("SNo");
                b.HasAnnotation("Relational:TableName", "AirportMonthlyData");
            });
        }
    }
}
