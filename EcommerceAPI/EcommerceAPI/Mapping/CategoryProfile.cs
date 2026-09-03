using AutoMapper;
using EcommerceAPI.Models;
using EcommerceAPI.Models.Dtos;

namespace EcommerceAPI.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        // From Right to Left and Left to Right
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CreateCategoryDto>().ReverseMap();
    }
}
