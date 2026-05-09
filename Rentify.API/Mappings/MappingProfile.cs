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
        }
    }
}
