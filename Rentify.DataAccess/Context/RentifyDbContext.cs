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
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Vehicle Configuration ──
            modelBuilder.Entity<Vehicle>(entity =>
            {

                // Relaciones

                // 1:N con VehicleType
                entity.HasOne(v => v.VehicleType)
                      .WithMany(t => t.Vehicles)
                      .HasForeignKey(v => v.VehicleTypeId);
                // 1:N con Branch
                entity.HasOne(v => v.Branch)
                      .WithMany(b => b.Vehicles)
                      .HasForeignKey(v => v.BranchId);
            });

            // ── Rental Configuration ──
            modelBuilder.Entity<Rental>(entity =>
            {
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
                      .WithMany()
                      .HasForeignKey(r => r.VehicleId)
                      .OnDelete(DeleteBehavior.Restrict);
                // 1:N con Customer
                entity.HasOne(r => r.Customer)
                      .WithMany(c => c.Rentals)
                      .HasForeignKey(r => r.CustomerId);
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
