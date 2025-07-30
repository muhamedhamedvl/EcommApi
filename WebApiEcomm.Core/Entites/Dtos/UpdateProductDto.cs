using Microsoft.AspNetCore.Http;

namespace WebApiEcomm.Core.Entites.Dtos
{
    public record UpdateProductDto : AddProductDto
    {
        public int Id { get; set; }
    }
}
