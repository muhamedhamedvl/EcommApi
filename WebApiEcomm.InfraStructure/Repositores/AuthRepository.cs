using Microsoft.AspNetCore.Identity;
using WebApiEcomm.Core.Entites.Dtos;
using WebApiEcomm.Core.Entites.Identity;
using WebApiEcomm.Core.Interfaces.Auth;
using WebApiEcomm.Core.Services;

namespace WebApiEcomm.InfraStructure.Repositores
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IGenrateToken genrateToken;

        public AuthRepository(UserManager<AppUser> userManager
            , IEmailService emailService
            , SignInManager<AppUser> signInManager
            , IGenrateToken genrateToken)

        {
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            this.genrateToken = genrateToken;
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
            //Send email confirmation
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, code, "active", "ActiveEmail", "Active your email,click on button to active");
            return user;
        }
        public async Task SendEmail(string email, string code, string component, string content, string message)
        {
            if (string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(code)
                || string.IsNullOrEmpty(component)
                || string.IsNullOrEmpty(content)
                || string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Email, code, component, content, and message must not be empty");
            }

            // Example: subject based on component
            string subject = $"Verification for {component}";

            // Format the content (replace placeholders in the template)
            string formattedContent = string.Format(content, email, code, component, message);

            var emailDto = new EmailDto(
                to: email,
                from: "mh1191128@gmail.com",
                subject: subject,
                content: formattedContent
            );

            await _emailService.SendEmail(emailDto);
        }
        public async Task<string> LoginAsync(LoginDto login)
        {
            if (login == null)
            {
                return null;
            }

            var finduser = await _userManager.FindByEmailAsync(login.Email);

            if (!finduser.EmailConfirmed)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(finduser);
                await SendEmail(finduser.Email, token, "active", "ActiveEmail", "Please active your email");
                return "Please confirm your email first, we have send activat to your E-mail";
            }

            var result = await _signInManager.CheckPasswordSignInAsync(finduser, login.Password, true);

            if (result.Succeeded)
            {
                return genrateToken.GetAndCreateTokenAsync(finduser);
            }

            return "please check your email and password, something went wrong";
        }
        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            var FindUser = await _userManager.FindByEmailAsync(email);
            if (FindUser is null)
            {
                return false;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(FindUser);

            await SendEmail(FindUser.Email, token, "Reset-Password", "Reset Password", "click on button to Reset");

            return true;
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            if (resetPasswordDto == null)
            {
                throw new ArgumentNullException(nameof(resetPasswordDto));
            }
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return false; 
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            return result.Succeeded;
        }
        public async Task<bool> ActiveAccount(ActiveDto activeDto)
        {
            var FindUser = await _userManager.FindByEmailAsync(activeDto.Email);
            if (FindUser is null)
            {
                return false;
            }

            var res = await _userManager.ConfirmEmailAsync(FindUser , activeDto.Token);
            if (res.Succeeded) 
            {
                return true;
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(FindUser);
            await SendEmail(FindUser.Email, token, "active", "ActiveEmail", "Please active your email");
            return false ;
        }
    }
}
