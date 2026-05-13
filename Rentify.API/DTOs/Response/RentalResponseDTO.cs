using Rentify.Domain.Enums;

namespace Rentify.API.DTOs.Response
{
    public class RentalResponseDTO
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public RentalStatus Status { get; set; }
    }
}
