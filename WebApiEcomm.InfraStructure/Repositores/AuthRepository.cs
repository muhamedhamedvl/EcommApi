using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Identity;
using WebApiEcomm.Core.Interfaces.Auth;

namespace WebApiEcomm.InfraStructure.Repositores
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<AppUser> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                throw new ArgumentNullException(nameof(registerDto));
            }
            if (await _userManager.FindByNameAsync(registerDto.UserName) != null)
            {
                throw new ArgumentException("Username already exists");
            }
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            {
                throw new ArgumentException("Email already exists");
            }
            AppUser user = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.FirstOrDefault()?.Description ?? "User creation failed");
            }
            //Send Active Email

            return user;
        }
    }
}
