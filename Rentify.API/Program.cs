using Microsoft.EntityFrameworkCore;
using Rentify.DataAccess.Context;
//using Rentify.Domain.Helpers;
using Rentify.DataAccess.Seeders;
using Rentify.DataAccess.Repositories;
using Rentify.Domain.Interfaces.Repositories;
using Rentify.Domain.Interfaces.Services;
using Rentify.Domain.Services;
using System.ComponentModel.Design;


var builder = WebApplication.CreateBuilder(args);

// ── Entity Framework Core ──
builder.Services.AddDbContext<RentifyDbContext>(options =>
 options.UseSqlServer(
 builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Repositories ──
builder.Services.AddScoped(typeof(IGenericRepository<>),
typeof(GenericRepository<>));
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleTypeRepository, VehicleTypeRepository>(); 
builder.Services.AddScoped<IVehicleMaintenanceRepository, VehicleMaintenanceRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();


// ── Services ──
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleTypeService, VehicleTypeService>();
builder.Services.AddScoped<IVehicleMaintenanceService, VehicleMaintenanceService>();
builder.Services.AddScoped<IRentalServices, RentalServices>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();

// ── AutoMapper ──
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// ── Controllers ──
builder.Services.AddControllers();

// ── Swagger ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Data Seeder ──
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RentifyDbContext>();
    await context.Database.MigrateAsync(); // Aplica las migraciones
    await DataSeeder.SeedAsync(context);   // Inserta los datos
}


// ── Middleware Pipeline ──
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
