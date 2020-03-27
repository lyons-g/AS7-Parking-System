using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AS7_Parking_System.Models
{
    public partial class ParkingDataBaseContext : DbContext
    {
        public ParkingDataBaseContext()
        {
        }

        public ParkingDataBaseContext(DbContextOptions<ParkingDataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Permit> Permit { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#pragma warning disable CS1030 // #warning directive
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ParkingDataBase;Integrated Security=True");
#pragma warning restore CS1030 // #warning directive
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permit>(entity =>
            {
                entity.HasIndex(e => e.VehicleId)
                    .IsUnique();

                entity.Property(e => e.Fee).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Premium).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Vehicle)
                    .WithOne(p => p.Permit)
                    .HasForeignKey<Permit>(d => d.VehicleId);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.VehicleModel).HasColumnName("Vehicle_Model");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
