using System;
using AutoMapper;
using EcommerceAPI.Models;
using EcommerceAPI.Models.Dtos;

namespace EcommerceAPI.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            // This line of code tells AutoMapper how to map a property that does not have the same name in both classes or that originates from a nested navigation property.
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ReverseMap();
        CreateMap<Product, CreateProductDto>().ReverseMap();
        CreateMap<Product, UpdateProductDto>().ReverseMap();
    }
}
