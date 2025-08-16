using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Identity;

namespace WebApiEcomm.Core.Services
{
    public interface IGenrateToken
    {
        public string GetAndCreateTokenAsync(AppUser appUser);
    }
}
