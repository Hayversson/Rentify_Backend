using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rentify.Domain.Interfaces.Services;
using Rentify.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Rentify.Domain.Services
{
    public class RentalStatusWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RentalStatusWorker> _logger;

        public RentalStatusWorker(IServiceScopeFactory scopeFactory, ILogger<RentalStatusWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RentalStatusWorker (Domain) started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var rentalServices = scope.ServiceProvider.GetRequiredService<IRentalServices>();
                    var vehicleService = scope.ServiceProvider.GetRequiredService<IVehicleService>();

                    var rentals = await rentalServices.GetAllAsync();
                    var now = System.DateTime.UtcNow;

                    var toComplete = rentals
                        .Where(r => r != null && r.Status == RentalStatus.Active && r.EndDate < now)
                        .ToList();

                    foreach (var rental in toComplete)
                    {
                        try
                        {
                            await rentalServices.UpdateStatusAsync(rental.Id, RentalStatus.Completed);
                            await vehicleService.UpdateStatusAsync(rental.VehicleId, VehicleStatus.Available);
                            _logger.LogInformation("Marked rental {Id} as Completed.", rental.Id);
                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError(ex, "Error updating rental status for {Id}", rental.Id);
                        }
                    }

                    // Activate pending rentals whose start date has arrived (and are not cancelled)
                    var toActivate = rentals
                        .Where(r => r != null && r.Status == RentalStatus.Pending && r.StartDate <= now)
                        .ToList();

                    foreach (var rental in toActivate)
                    {
                        try
                        {
                            // Transition Pending -> Active
                            await rentalServices.UpdateStatusAsync(rental.Id, RentalStatus.Active);
                            // Mark vehicle as Rented
                            await vehicleService.UpdateStatusAsync(rental.VehicleId, VehicleStatus.Rented);
                            _logger.LogInformation("Activated rental {Id} and marked vehicle {VehicleId} as Rented.", rental.Id, rental.VehicleId);
                        }
                        catch (System.Exception ex)
                        {
                            _logger.LogError(ex, "Error activating rental {Id}", rental.Id);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Error in RentalStatusWorker loop.");
                }

                try
                {
                    await Task.Delay(60_000, stoppingToken); // run every 60 seconds
                }
                catch (TaskCanceledException)
                {
                    // Shutdown requested, exit cleanly.
                }
            }
        }
    }
}
