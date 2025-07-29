using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Product;

namespace WebApiEcomm.InfraStructure.Data.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(40);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.NewPrice).IsRequired().HasColumnType("decimal(18,2)");
            builder.HasData(
                new Product
                {
                    Id = 1,
                    Name = "Smartphone",
                    Description = "Latest model with advanced features",
                    NewPrice = 699.99m,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Laptop",
                    Description = "High performance laptop for professionals",
                    NewPrice = 1299.99m,
                    CategoryId = 1
                }
            );
        }
    }
}
