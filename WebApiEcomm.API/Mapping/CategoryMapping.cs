using AutoMapper;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Product;

namespace WebApiEcomm.API.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();
        }
    }
}
