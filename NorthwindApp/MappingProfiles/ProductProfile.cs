using AutoMapper;
using NorthwindApp.DTO;
using NorthwindApp.Models;

namespace NorthwindApp.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(destination => destination.Id, source => source.MapFrom(p => p.ProductId))
                .ForMember(destination => destination.Name, source => source.MapFrom(p => p.ProductName))
                .ForMember(destination => destination.SupplierName, source => source.MapFrom(p => p.Supplier.CompanyName))
                .ForMember(destination => destination.CategoryName, source => source.MapFrom(p => p.Category.CategoryName));
        }
    }
}
