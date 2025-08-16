using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEcomm.Core.Entites.Dtos
{
    public record ResetPasswordDto : LoginDto
    {
        public string Token { get; init; }
    }
}
