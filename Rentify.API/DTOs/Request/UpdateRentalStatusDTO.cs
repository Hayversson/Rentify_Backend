using Rentify.Domain.Enums;

namespace Rentify.API.DTOs.Request
{
    public class UpdateRentalStatusDTO
    {
        public RentalStatus Status { get; set; }
    }
}
