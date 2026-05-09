using Microsoft.EntityFrameworkCore;
using Rentify.DataAccess.Repositories;
using Rentify.Domain.Interfaces.Repositories;
using Rentify.DataAccess.Context;
using Rentify.DataAccess.Seeders;
//using Rentify.Domain.Helpers;
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
builder.Services.AddScoped<IBranchRepository, BranchRepository>();


// ── Services ──
builder.Services.AddScoped<IBranchService, BranchService>();

// ── AutoMapper ──
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// ── Controllers ──
builder.Services.AddControllers();

// ── Swagger ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// ── Data Seeder ──
/*using (var scope = app.Services.CreateScope()) //Scoped, singleton y transient
{
    var context = scope.ServiceProvider
        .GetRequiredService<RentifyDbContext>();

    await context.Database.MigrateAsync(); // Crea la BD + aplica migraciones
    await DataSeeder.SeedAsync(context);
}*/


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