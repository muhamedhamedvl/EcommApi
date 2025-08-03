using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Product;
using WebApiEcomm.Core.Interfaces;
using WebApiEcomm.Core.Services;
using WebApiEcomm.Core.Sharing;
using WebApiEcomm.InfraStructure.Data;

namespace WebApiEcomm.InfraStructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;

        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService)
            : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }
        public async Task<IEnumerable<ProductDto>> GetAllAsync(ProductParams productParams)
        {
            var query = context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .AsNoTracking();

            //Filtering By CategoryId
            if (productParams.CategoryId.HasValue)
            {
                query = query.Where(m => m.CategoryId == productParams.CategoryId);
            }
            //Fiter By Price
            if (!string.IsNullOrEmpty(productParams.sort))
            {
                query = productParams.sort switch
                {
                    "PriceAsn" => query.OrderBy(m => m.NewPrice),
                    "PriceDes" => query.OrderByDescending(m => m.NewPrice),
                    _ => query.OrderBy(m => m.Name),
                };
            }
            //Pigenation
            query = query.Skip(productParams.PageSize * (productParams.PageNumber - 1)).Take(productParams.PageSize);

            var res = mapper.Map<List<ProductDto>>(query);
            return res;
        }
        public async Task<bool> AddAsync(AddProductDto productDTO)
        {
            if (productDTO == null) return false;

            var product = mapper.Map<Product>(productDTO);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var imagePaths = await imageManagementService.AddImageAsync(productDTO.Photo, productDTO.Name);

            var photos = imagePaths.Select(a => new Photo
            {
                ImageName = a,
                ProductId = product.Id
            }).ToList();

            await context.Photos.AddRangeAsync(photos);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateProductDto updateproductDTO)
        {
            if (updateproductDTO is null) return false;

            var product = await context.Products
                .Include(p => p.Category)
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(p => p.Id == updateproductDTO.Id);

            if (product == null) return false;

            mapper.Map(updateproductDTO, product);

            var oldPhotos = await context.Photos
                .Where(p => p.ProductId == product.Id)
                .ToListAsync();

            foreach (var item in oldPhotos)
            {
                await imageManagementService.DeleteImageAsync(item.ImageName);
            }

            context.Photos.RemoveRange(oldPhotos);

            var newImagePaths = await imageManagementService.AddImageAsync(updateproductDTO.Photo, updateproductDTO.Name);

            var newPhotos = newImagePaths.Select(a => new Photo
            {
                ImageName = a,
                ProductId = product.Id
            }).ToList();

            await context.Photos.AddRangeAsync(newPhotos);
            await context.SaveChangesAsync();

            return true;
        }

        async Task IProductRepository.DeleteAsync(Product product)
        {
            var photo = await 
                 context
                .Photos
                .Where(p => p.ProductId == product.Id)
                .ToListAsync();

            foreach (var item in photo)
            {
                await imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Products.Remove(product);
            context.Photos.RemoveRange(photo);
            await context.SaveChangesAsync();
        }
    }
}
