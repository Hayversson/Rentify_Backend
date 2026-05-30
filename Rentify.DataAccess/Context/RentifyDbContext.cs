using Microsoft.EntityFrameworkCore;
using Rentify.Domain.Entities;

namespace Rentify.DataAccess.Context
{
    public class RentifyDbContext : DbContext
    {
        public RentifyDbContext(DbContextOptions<RentifyDbContext> options)
        : base(options)
        {
        }

        public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();
        public DbSet<Vehicle> Vehicles  => Set<Vehicle>();
        public DbSet<VehicleMaintenance> VehicleMaintenances  => Set<VehicleMaintenance>();
        public DbSet<Payment> Payments  => Set<Payment>();
        public DbSet<Customer> Customers  => Set<Customer>();
        public DbSet<Rental> Rentals  => Set<Rental>();
        public DbSet<Branch> Branches  => Set<Branch>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── VehicleType Configuration ──
            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.HasKey(vt => vt.Id);
                entity.Property(vt => vt.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(vt => vt.PricePerDay)
                      .IsRequired()
                      .HasPrecision(18, 2);
                entity.Property(t => t.CreatedAt)
                      .IsRequired();
                entity.Property(t => t.UpdatedAt)
                      .IsRequired(false);
                entity.HasIndex(t => t.Name)
                      .IsUnique();
            });

            // ── Vehicle Configuration ──
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Plate)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(v => v.Model)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(v => v.Year)
                      .IsRequired();
                entity.Property(v => v.Status)
                      .IsRequired();
                entity.Property(v => v.CreatedAt)
                      .IsRequired();
                entity.Property(v => v.UpdatedAt)
                      .IsRequired(false);

                // Relaciones

                // 1:N con VehicleType
                entity.HasOne(v => v.VehicleType)
                      .WithMany(vt => vt.Vehicles)
                      .HasForeignKey(v => v.VehicleTypeId)
                      .OnDelete(DeleteBehavior.Restrict); 
                // 1:N con Branch
                entity.HasOne(v => v.Branch)
                      .WithMany(b => b.Vehicles)
                      .HasForeignKey(v => v.BranchId);
            });

            // ── VehicleMaintenance Configuration ──
            modelBuilder.Entity<VehicleMaintenance>(entity =>
            {
                entity.HasKey(vm => vm.Id);
                entity.Property(vm => vm.VehicleId)
                      .IsRequired();
                entity.Property(vm => vm.Description)
                      .IsRequired()
                      .HasMaxLength(500);
                entity.Property(vm => vm.StartDate)
                      .IsRequired();
                entity.Property(vm => vm.EndDate)
                      .IsRequired(false);
                entity.Property(vm => vm.CreatedAt)
                      .IsRequired();
                entity.Property(vm => vm.UpdatedAt)
                      .IsRequired(false);

                // Relaciones
                // 1:N con Vehicle
                entity.HasOne(vm => vm.Vehicle)
                      .WithMany(v => v.Maintenances)
                      .HasForeignKey(vm => vm.VehicleId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            // ── Payment Configuration ──
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.RentalId)
                      .IsRequired();
                entity.Property(p => p.Amount)
                      .IsRequired()
                      .HasPrecision(18, 2);
                entity.Property(p => p.Method)
                      .IsRequired();
                entity.Property(p => p.CreatedAt)
                      .IsRequired();
                entity.Property(p => p.UpdatedAt)
                      .IsRequired(false);
            });

            // ── Rental Configuration ──
            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.PickupBranchId)
                      .IsRequired();
                entity.Property(b => b.ReturnBranchId)
                      .IsRequired();
                entity.Property(b => b.StartDate)
                      .IsRequired();
                entity.Property(b => b.EndDate)
                      .IsRequired();
                entity.Property(b => b.TotalCost)
                      .IsRequired()
                      .HasPrecision(18, 2);
                entity.Property(b => b.Status)
                       .IsRequired();
                entity.Property(b => b.CreatedAt)
                      .IsRequired();
                entity.Property(b => b.UpdatedAt)
                      .IsRequired(false);
                // Relaciones

                // 1:N con Branch (Pickup y Return) multiples FK a la misma tabla Branch
                entity.HasOne(r => r.PickupBranch)
                      .WithMany()
                      .HasForeignKey(r => r.PickupBranchId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(r => r.ReturnBranch)
                      .WithMany()
                      .HasForeignKey(r => r.ReturnBranchId)
                      .OnDelete(DeleteBehavior.Restrict);
                // 1:N con Vehicle
                entity.HasOne(r => r.Vehicle)
                      .WithMany(v => v.Rentals)
                      .HasForeignKey(r => r.VehicleId)
                      .OnDelete(DeleteBehavior.Restrict);
                // 1:N con Customer
                entity.HasOne(r => r.Customer)
                      .WithMany(c => c.Rentals)
                      .HasForeignKey(r => r.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade); 
                // 1:1 con Payment
                entity.HasOne(r => r.Payment)
                      .WithOne(p => p.Rental)
                      .HasForeignKey<Payment>(p => p.RentalId);
            });

            // ── Customer Configuration ──
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.FirstName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(c => c.LastName)
                      .IsRequired()
                      .HasMaxLength(80);
                entity.Property(c => c.Email)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(c => c.Phone)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(c => c.LicenseNumber)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(c => c.LicenseExpirationDate)
                      .IsRequired();
                entity.Property(c => c.CreatedAt)
                      .IsRequired();
                entity.Property(c => c.UpdatedAt)
                      .IsRequired(false);
                // Índices únicos 
                entity.HasIndex(c => c.Email).IsUnique();
                entity.HasIndex(c => c.LicenseNumber).IsUnique();
            });

            // ── Branch Configuration ──
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(b => b.City)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(b => b.Address)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(b => b.Phone)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(b => b.OpeningTime)
                      .IsRequired();
                entity.Property(b => b.ClosingTime)
                      .IsRequired();
                entity.Property(b => b.CreatedAt)
                      .IsRequired();
                entity.Property(b => b.UpdatedAt)
                      .IsRequired(false);

                // Índices
                entity.HasIndex(b => b.Name).IsUnique();
            });
        }
    }
}
