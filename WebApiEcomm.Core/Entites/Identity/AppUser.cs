
using Microsoft.AspNetCore.Identity;

namespace WebApiEcomm.Core.Entites.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public virtual Address Address { get; set; }
    }
}
