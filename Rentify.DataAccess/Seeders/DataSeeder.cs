using Microsoft.EntityFrameworkCore;
using Rentify.Domain.Entities;
using Rentify.Domain.Enums;
using Rentify.DataAccess.Context;

namespace Rentify.DataAccess.Seeders
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(RentifyDbContext context)
        {
            // Solo ejecutar si ya existen datos
            if (await context.VehicleTypes.AnyAsync()) return;

            // ═══ 1. VEHICLE TYPES ═══
            var vehicleTypes = new List<VehicleType>
            {
                new() { Name = "Economy", PricePerDay = 45, CreatedAt = DateTime.UtcNow },
                new() { Name = "Sedan", PricePerDay = 70, CreatedAt = DateTime.UtcNow },
                new() { Name = "SUV", PricePerDay = 110, CreatedAt = DateTime.UtcNow },
                new() { Name = "Luxury", PricePerDay = 220, CreatedAt = DateTime.UtcNow },
                new() { Name = "Pickup", PricePerDay = 130, CreatedAt = DateTime.UtcNow },
                new() { Name = "Sports", PricePerDay = 300, CreatedAt = DateTime.UtcNow },
                new() { Name = "Electric", PricePerDay = 160, CreatedAt = DateTime.UtcNow },
                new() { Name = "Van", PricePerDay = 140, CreatedAt = DateTime.UtcNow },
            };

            context.VehicleTypes.AddRange(vehicleTypes);
            await context.SaveChangesAsync();

            // ═══ 2. BRANCHES ═══
            var branches = new List<Branch>
            {
                new() { Name = "Miami Central", City = "Miami", Address = "1200 Ocean Drive", Phone = "3051112233", OpeningTime = new TimeSpan(7,0,0), ClosingTime = new TimeSpan(20,0,0), CreatedAt = DateTime.UtcNow },
                new() { Name = "Downtown Miami", City = "Miami", Address = "55 Brickell Ave", Phone = "3052223344", OpeningTime = new TimeSpan(7,0,0), ClosingTime = new TimeSpan(21,0,0), CreatedAt = DateTime.UtcNow },
                new() { Name = "Orlando Airport", City = "Orlando", Address = "1 Airport Blvd", Phone = "4073334455", OpeningTime = new TimeSpan(6,0,0), ClosingTime = new TimeSpan(23,0,0), CreatedAt = DateTime.UtcNow },
                new() { Name = "Tampa Bay", City = "Tampa", Address = "89 Bay Street", Phone = "8134445566", OpeningTime = new TimeSpan(8,0,0), ClosingTime = new TimeSpan(20,0,0), CreatedAt = DateTime.UtcNow },
                new() { Name = "Fort Lauderdale", City = "Fort Lauderdale", Address = "200 Beach Blvd", Phone = "9545556677", OpeningTime = new TimeSpan(7,0,0), ClosingTime = new TimeSpan(20,0,0), CreatedAt = DateTime.UtcNow },
                new() { Name = "Jacksonville North", City = "Jacksonville", Address = "400 Main Avenue", Phone = "9046667788", OpeningTime = new TimeSpan(8,0,0), ClosingTime = new TimeSpan(19,0,0), CreatedAt = DateTime.UtcNow },
            };

            context.Branches.AddRange(branches);
            await context.SaveChangesAsync();

            // ═══ 3. CUSTOMERS ═══
            var customers = new List<Customer>
            {
                new() { FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com", Phone = "3057001001", LicenseNumber = "LIC1001", LicenseExpirationDate = DateTime.UtcNow.AddYears(4), CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Emily", LastName = "Johnson", Email = "emily.johnson@gmail.com", Phone = "3057001002", LicenseNumber = "LIC1002", LicenseExpirationDate = DateTime.UtcNow.AddYears(5), CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Michael", LastName = "Smith", Email = "michael.smith@gmail.com", Phone = "3057001003", LicenseNumber = "LIC1003", LicenseExpirationDate = DateTime.UtcNow.AddYears(3), CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Sophia", LastName = "Brown", Email = "sophia.brown@gmail.com", Phone = "3057001004", LicenseNumber = "LIC1004", LicenseExpirationDate = DateTime.UtcNow.AddYears(2), CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Daniel", LastName = "Wilson", Email = "daniel.wilson@gmail.com", Phone = "3057001005", LicenseNumber = "LIC1005", LicenseExpirationDate = DateTime.UtcNow.AddYears(6), CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Olivia", LastName = "Martinez", Email = "olivia.martinez@gmail.com", Phone = "3057001006", LicenseNumber = "LIC1006", LicenseExpirationDate = DateTime.UtcNow.AddYears(5), CreatedAt = DateTime.UtcNow },
                new() { FirstName = "James", LastName = "Taylor", Email = "james.taylor@gmail.com", Phone = "3057001007", LicenseNumber = "LIC1007", LicenseExpirationDate = DateTime.UtcNow.AddYears(3), CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Ava", LastName = "Anderson", Email = "ava.anderson@gmail.com", Phone = "3057001008", LicenseNumber = "LIC1008", LicenseExpirationDate = DateTime.UtcNow.AddYears(4), CreatedAt = DateTime.UtcNow },
                new() { FirstName = "William", LastName = "Thomas", Email = "william.thomas@gmail.com", Phone = "3057001009", LicenseNumber = "LIC1009", LicenseExpirationDate = DateTime.UtcNow.AddYears(5), CreatedAt = DateTime.UtcNow },
                new() { FirstName = "Mia", LastName = "Jackson", Email = "mia.jackson@gmail.com", Phone = "3057001010", LicenseNumber = "LIC1010", LicenseExpirationDate = DateTime.UtcNow.AddYears(2), CreatedAt = DateTime.UtcNow },
            };

            context.Customers.AddRange(customers);
            await context.SaveChangesAsync();

            // ═══ 4. VEHICLES ═══
            var vehicles = new List<Vehicle>
            {
                new() { Plate="ABC101", Model="Toyota Corolla", Year=2022, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[0].Id, BranchId=branches[0].Id },
                new() { Plate="ABC102", Model="Hyundai Accent", Year=2021, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[0].Id, BranchId=branches[1].Id },
                new() { Plate="ABC103", Model="Kia Rio", Year=2020, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[0].Id, BranchId=branches[2].Id },
                new() { Plate="SED201", Model="Honda Civic", Year=2023, Status=VehicleStatus.Rented, VehicleTypeId=vehicleTypes[1].Id, BranchId=branches[0].Id },
                new() { Plate="SED202", Model="Mazda 3", Year=2022, Status=VehicleStatus.Rented, VehicleTypeId=vehicleTypes[1].Id, BranchId=branches[1].Id },
                new() { Plate="SED203", Model="Nissan Sentra", Year=2021, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[1].Id, BranchId=branches[3].Id },
                new() { Plate="SUV301", Model="Toyota RAV4", Year=2024, Status=VehicleStatus.Rented, VehicleTypeId=vehicleTypes[2].Id, BranchId=branches[0].Id },
                new() { Plate="SUV302", Model="Ford Explorer", Year=2023, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[2].Id, BranchId=branches[4].Id },
                new() { Plate="SUV303", Model="Chevrolet Tahoe", Year=2022, Status=VehicleStatus.InMaintenance, VehicleTypeId=vehicleTypes[2].Id, BranchId=branches[5].Id },
                new() { Plate="LUX401", Model="BMW 5 Series", Year=2024, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[3].Id, BranchId=branches[0].Id },
                new() { Plate="LUX402", Model="Mercedes C300", Year=2023, Status=VehicleStatus.Rented, VehicleTypeId=vehicleTypes[3].Id, BranchId=branches[1].Id },
                new() { Plate="LUX403", Model="Audi A6", Year=2022, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[3].Id, BranchId=branches[2].Id },
                new() { Plate="PIC501", Model="Ford Ranger", Year=2024, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[4].Id, BranchId=branches[3].Id },
                new() { Plate="PIC502", Model="Toyota Hilux", Year=2023, Status=VehicleStatus.InMaintenance, VehicleTypeId=vehicleTypes[4].Id, BranchId=branches[4].Id },
                new() { Plate="ELE601", Model="Tesla Model 3", Year=2024, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[5].Id, BranchId=branches[0].Id },
                new() { Plate="ELE602", Model="Tesla Model Y", Year=2024, Status=VehicleStatus.Rented, VehicleTypeId=vehicleTypes[5].Id, BranchId=branches[1].Id },
                new() { Plate="SPO701", Model="Chevrolet Camaro", Year=2023, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[6].Id, BranchId=branches[2].Id },
                new() { Plate="SPO702", Model="Ford Mustang", Year=2024, Status=VehicleStatus.Rented, VehicleTypeId=vehicleTypes[6].Id, BranchId=branches[3].Id },
                new() { Plate="VAN801", Model="Mercedes Sprinter", Year=2022, Status=VehicleStatus.Available, VehicleTypeId=vehicleTypes[7].Id, BranchId=branches[4].Id },
                new() { Plate="VAN802", Model="Ford Transit", Year=2021, Status=VehicleStatus.InMaintenance, VehicleTypeId=vehicleTypes[7].Id, BranchId=branches[5].Id }
             };

            context.Vehicles.AddRange(vehicles);
            await context.SaveChangesAsync();


            // ═══ 5. VEHICLE MAINTENANCES ═══
            var maintenances = new List<VehicleMaintenance>
            {
                new() { VehicleId=vehicles[8].Id, Description="Cambio completo de frenos delanteros y traseros", StartDate=DateTime.UtcNow.AddDays(-3), EndDate=DateTime.UtcNow.AddDays(2) },
                new() { VehicleId=vehicles[13].Id, Description="Mantenimiento preventivo de motor y suspensión", StartDate=DateTime.UtcNow.AddDays(-2), EndDate=DateTime.UtcNow.AddDays(4) },
                new() { VehicleId=vehicles[19].Id, Description="Cambio de aceite, filtros y revisión eléctrica", StartDate=DateTime.UtcNow.AddDays(-1), EndDate=DateTime.UtcNow.AddDays(3) }
            };

            context.VehicleMaintenances.AddRange(maintenances);
            await context.SaveChangesAsync();

            // ═══ 6. RENTALS ═══
            var rentals = new List<Rental>
            {
                new() { CustomerId=customers[0].Id, VehicleId=vehicles[3].Id, PickupBranchId=branches[0].Id, ReturnBranchId=branches[1].Id, StartDate=DateTime.UtcNow.AddDays(-2), EndDate=DateTime.UtcNow.AddDays(5), TotalCost=420, Status=RentalStatus.Active },
                new() { CustomerId=customers[1].Id, VehicleId=vehicles[4].Id, PickupBranchId=branches[1].Id, ReturnBranchId=branches[1].Id, StartDate=DateTime.UtcNow.AddDays(-1), EndDate=DateTime.UtcNow.AddDays(4), TotalCost=350, Status=RentalStatus.Active },
                new() { CustomerId=customers[2].Id, VehicleId=vehicles[6].Id, PickupBranchId=branches[0].Id, ReturnBranchId=branches[2].Id, StartDate=DateTime.UtcNow.AddDays(-3), EndDate=DateTime.UtcNow.AddDays(6), TotalCost=1080, Status=RentalStatus.Active },
                new() { CustomerId=customers[3].Id, VehicleId=vehicles[10].Id, PickupBranchId=branches[1].Id, ReturnBranchId=branches[3].Id, StartDate=DateTime.UtcNow.AddDays(-1), EndDate=DateTime.UtcNow.AddDays(2), TotalCost=750, Status=RentalStatus.Active },
                new() { CustomerId=customers[4].Id, VehicleId=vehicles[15].Id, PickupBranchId=branches[1].Id, ReturnBranchId=branches[0].Id, StartDate=DateTime.UtcNow.AddDays(-4), EndDate=DateTime.UtcNow.AddDays(3), TotalCost=1260, Status=RentalStatus.Active },
                new() { CustomerId=customers[5].Id, VehicleId=vehicles[17].Id, PickupBranchId=branches[3].Id, ReturnBranchId=branches[5].Id, StartDate=DateTime.UtcNow.AddDays(-2), EndDate=DateTime.UtcNow.AddDays(1), TotalCost=960, Status=RentalStatus.Active }
            };

            context.Rentals.AddRange(rentals);
            await context.SaveChangesAsync();

            // ═══ 7. PAYMENTS ═══
            var payments = new List<Payment>
            {
                new() { RentalId = rentals[0].Id, Amount = 550, Method = "CreditCard", CreatedAt = DateTime.UtcNow },
                new() { RentalId = rentals[1].Id, Amount = 320, Method = "DebitCard", CreatedAt = DateTime.UtcNow },
                new() { RentalId = rentals[2].Id, Amount = 450, Method = "Cash", CreatedAt = DateTime.UtcNow },
                new() { RentalId = rentals[3].Id, Amount = 1200, Method = "CreditCard", CreatedAt = DateTime.UtcNow },
                new() { RentalId = rentals[4].Id, Amount = 1800, Method = "BankTransfer", CreatedAt = DateTime.UtcNow },
            };

            context.Payments.AddRange(payments);
            await context.SaveChangesAsync();
        }
    }
}