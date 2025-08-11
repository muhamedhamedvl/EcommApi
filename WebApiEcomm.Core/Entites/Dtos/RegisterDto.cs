using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEcomm.Core.Entites.Dtos
{
    public record RegisterDto : LoginDto
    {
        public string UserName { get; init; }
    }
}
