using Microsoft.EntityFrameworkCore;
using Rentify.Domain.Entities;

namespace Rentify.DataAccess.Context
{
    internal class RentifyDbContext : DbContext
    {
        public RentifyDbContext(DbContextOptions<RentifyDbContext> options)
        : base(options)
        {
        }
    }
}
