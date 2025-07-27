using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiEcomm.Core.Entites.Product;
using WebApiEcomm.Core.Interfaces;
using WebApiEcomm.InfraStructure.Repositories;
namespace WebApiEcomm.InfraStructure.Repositores
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DbContext context) : base(context)
        {
        }
    }
}
