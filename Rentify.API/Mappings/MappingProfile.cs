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
            //Vehicle mappings
            CreateMap<VehicleRequestDTO, Vehicle>();
            CreateMap<Vehicle, VehicleResponseDTO>();

            //VehicleType mappings
            CreateMap<VehicleTypeRequestDTO, VehicleType>();
            CreateMap<VehicleType, VehicleTypeResponseDTO>();

            // VehicleMaintenance mappings
            CreateMap<VehicleMaintenanceRequestDTO, VehicleMaintenance>();
            CreateMap<VehicleMaintenance, VehicleMaintenanceResponseDTO>();
        }
    }
}
