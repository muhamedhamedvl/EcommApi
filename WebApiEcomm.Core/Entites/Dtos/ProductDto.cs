using System.Collections.Generic;

namespace WebApiEcomm.Core.Entites.Dtos
{
    public record ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public List<string> Photos { get; set; }
        public string CategoryName { get; set; }
    }
}
