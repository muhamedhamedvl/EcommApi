using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEcomm.Core.Entites.Dtos
{
    public record CategoryDto(string Name , String Description);
    public record UpdateCategoryDto(int Id, string Name, string Description);
}
