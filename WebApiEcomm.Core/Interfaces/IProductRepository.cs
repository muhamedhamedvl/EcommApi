using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Product;
using WebApiEcomm.Core.Sharing;
namespace WebApiEcomm.Core.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> AddAsync(AddProductDto productDTO);
        Task<bool> UpdateAsync(UpdateProductDto updateproductDTO);
        Task DeleteAsync(Product product);
        Task<IEnumerable<ProductDto>> GetAllAsync(ProductParams productParams);
    }
}
