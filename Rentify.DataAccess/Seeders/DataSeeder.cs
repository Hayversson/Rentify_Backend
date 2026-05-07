using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using Rentify.DataAccess.Context;

namespace Rentify.DataAccess.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(RentifyDbContext context)
        {
            // Evitar duplicados
            if (context.VehicleTypes.Any()) return;

            // =========================
            // Branches
            // =========================
            var branches = new List<Branch>
            {
                new Branch
                {
                    Name = "Central Branch",
                    City = "Miami",
                    CreatedAt = DateTime.UtcNow
                },
                new Branch
                {
                    Name = "Airport Branch",
                    City = "Orlando",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Branches.AddRangeAsync(branches);
            await context.SaveChangesAsync();

            // =========================
            // Vehicle Types
            // =========================
            var vehicleTypes = new List<VehicleType>
            {
                new VehicleType
                {
                    Name = "Sedan",
                    PricePerDay = 50,
                    CreatedAt = DateTime.UtcNow
                },
                new VehicleType
                {
                    Name = "SUV",
                    PricePerDay = 90,
                    CreatedAt = DateTime.UtcNow
                },
                new VehicleType
                {
                    Name = "Truck",
                    PricePerDay = 120,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.VehicleTypes.AddRangeAsync(vehicleTypes);
            await context.SaveChangesAsync();

            // =========================
            // Customers
            // =========================
            var customers = new List<Customer>
            {
                new Customer
                {
                    Name = "John Doe",
                    LicenseNumber = "LIC12345",
                    CreatedAt = DateTime.UtcNow
                },
                new Customer
                {
                    Name = "Jane Smith",
                    LicenseNumber = "LIC67890",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();

            // =========================
            // Vehicles
            // =========================
            var vehicles = new List<Vehicle>
            {
                new Vehicle
                {
                    Plate = "ABC123",
                    Model = "Toyota Corolla",
                    Year = 2022,
                    Status = VehicleStatus.Available,
                    VehicleTypeId = vehicleTypes[0].Id,
                    BranchId = branches[0].Id,
                    CreatedAt = DateTime.UtcNow
                },
                new Vehicle
                {
                    Plate = "XYZ789",
                    Model = "Ford Explorer",
                    Year = 2023,
                    Status = VehicleStatus.Available,
                    VehicleTypeId = vehicleTypes[1].Id,
                    BranchId = branches[1].Id,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Vehicles.AddRangeAsync(vehicles);
            await context.SaveChangesAsync();

            // =========================
            // Vehicle Maintenances
            // =========================

        }
    }
}
