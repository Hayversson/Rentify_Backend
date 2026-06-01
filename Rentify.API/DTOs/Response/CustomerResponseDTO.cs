using Rentify.Domain.Entities;

namespace Rentify.API.DTOs.Response
{
    public class CustomerResponseDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime LicenseExpirationDate { get; set; }
        public ICollection<RentalResponseDTO>? Rentals { get; set; }
    }
}
