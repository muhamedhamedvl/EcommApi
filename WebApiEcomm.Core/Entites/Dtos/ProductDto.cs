using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Product;

namespace WebApiEcomm.Core.Entites.Dtos
{
    public record ProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public virtual List<Photo> Photos { get; set; }
        public string CategoryName { get; set; }
    }

}
