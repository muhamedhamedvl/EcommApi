using AutoMapper;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Product;

namespace WebApiEcomm.API.Mapping
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos.Select(p => p.ImageName).ToList()));

            CreateMap<Photo, PhotoDto>().ReverseMap();

            CreateMap<AddProductDto, Product>()
                .ForMember(m => m.Photos, op => op.Ignore())
                .ReverseMap();

            CreateMap<UpdateProductDto, Product>()
                .ForMember(m => m.Photos, op => op.Ignore())
                .ReverseMap();
        }
    }
}
