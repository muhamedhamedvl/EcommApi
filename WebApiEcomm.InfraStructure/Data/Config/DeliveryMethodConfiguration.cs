using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Order;

namespace WebApiEcomm.InfraStructure.Data.Config
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(m => m.Price).HasColumnType("decimal(18,2)");
            builder.HasData(
                new DeliveryMethod
                {
                    Id = 1,
                    Name = "Standard",
                    DeliveryTime = "3-5 days",
                    Description = "Delivered within 3-5 business days",
                    Price = 5.99m
                },
                new DeliveryMethod
                {
                    Id = 2,
                    Name = "Express",
                    DeliveryTime = "1-2 days",
                    Description = "Delivered within 1-2 business days",
                    Price = 9.99m
                });
        }
    }
}
