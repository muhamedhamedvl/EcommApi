using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEcomm.Core.Services
{
    public interface IImageManagementService
    {
        Task<List<string>> AddImageAsync(IFormFileCollection Files , string src);
        Task<string> DeleteImageAsync(string src);
    }
}
