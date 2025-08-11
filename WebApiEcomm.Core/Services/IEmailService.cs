using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Entites.Dtos;

namespace WebApiEcomm.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto emailDto);
    }
}
