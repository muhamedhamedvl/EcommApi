using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEcomm.Core.Entites.Dtos
{
    public record RegisterDto
    {
        public string UserName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string ConfirmPassword { get; init; }
        public string PhoneNumber { get; init; }
        public string Address { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string ZipCode { get; init; }
    }
}
