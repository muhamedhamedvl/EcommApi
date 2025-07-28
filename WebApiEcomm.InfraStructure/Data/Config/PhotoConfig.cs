using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Product;

namespace WebApiEcomm.InfraStructure.Data.Config
{
    public class PhotoConfig : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasData(
                new Photo { Id = 1, ImageName = "smartphone.jpg", ProductId = 1 },
                new Photo { Id = 2, ImageName = "laptop.jpg", ProductId = 2 }
            );
        }
    }
}
