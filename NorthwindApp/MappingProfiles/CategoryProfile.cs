using AutoMapper;
using NorthwindApp.DTO;
using NorthwindApp.Models;

namespace NorthwindApp.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(destination => destination.Id, source => source.MapFrom(c => c.CategoryId))
                .ForMember(destination => destination.Name, source => source.MapFrom(c => c.CategoryName));
        }
    }
}
