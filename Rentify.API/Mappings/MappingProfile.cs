using AutoMapper;
using Rentify.API.DTOs.Request;
using Rentify.API.DTOs.Response;
using Rentify.Domain.Entities;

namespace Rentify.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Branch Mapping 
            CreateMap<BranchRequestDto, Branch>();
            CreateMap<Branch, BranchResponseDto>();

            // Customer Mapping
            CreateMap<CustomerRequestDto, Customer>();
            CreateMap<Customer, CustomerResponseDto>();

            //Vehicle mappings
            CreateMap<VehicleRequestDTO, Vehicle>();
            CreateMap<Vehicle, VehicleResponseDTO>();

            //VehicleType mappings
            CreateMap<VehicleTypeRequestDTO, VehicleType>();
            CreateMap<VehicleType, VehicleTypeResponseDTO>();

            // VehicleMaintenance mappings
            CreateMap<VehicleMaintenanceRequestDTO, VehicleMaintenance>();
            CreateMap<VehicleMaintenance, VehicleMaintenanceResponseDTO>();

            // Rental mappings
            CreateMap<RentalRequestDTO, Rental>();
            CreateMap<Rental, RentalResponseDTO>();

            // Payment mappings
            CreateMap<PaymentRequestDTO, Payment>();
            CreateMap<Payment, PaymentResponseDTO>();
        }
    }
}
