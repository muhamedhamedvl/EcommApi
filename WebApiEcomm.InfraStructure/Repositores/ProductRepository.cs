using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Product;
using WebApiEcomm.Core.Interfaces;
using WebApiEcomm.Core.Services;
using WebApiEcomm.InfraStructure.Data;

namespace WebApiEcomm.InfraStructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        public ProductRepository(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDto productDTO)
        {
            if (productDTO == null)return false;
            var product = mapper.Map<Product>(productDTO);
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var ImagePath = await imageManagementService.AddImageAsync(productDTO.Photo, productDTO.Name);

            var Photo = ImagePath.Select(a => new Photo
            {
                ImageName = a,
                ProductId = product.Id
            }).ToList();
            context.Photos.AddRangeAsync(Photo);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
