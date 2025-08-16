using AutoMapper;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Order;
namespace WebApiEcomm.API.Mapping
{
    public class OrderMapping:Profile
    {
        public OrderMapping()
        {
            CreateMap<ShippingAddress, ShippingAddressDto>().ReverseMap();

        }
    }
}
