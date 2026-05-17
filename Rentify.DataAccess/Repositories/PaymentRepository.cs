using Rentify.Domain.Entities;
using Rentify.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rentify.DataAccess.Context;

namespace Rentify.DataAccess.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(RentifyDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Payment>> GetByCustomer(int id)
        {
            var payments = await _dbSet.Where(p => p.Rental.CustomerId == id).ToListAsync();
            return payments;
        }

        public async Task<IEnumerable<Payment>> GetByVehicule(int id)
        {
            var payments = await _dbSet.Where(p => p.Rental.VehicleId == id).ToListAsync();
            return payments;
        }

        //AGREGAR GET WITH INCLUDES
    }
}
