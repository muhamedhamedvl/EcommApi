using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;
using WebApiEcomm.Core.Interfaces;
using WebApiEcomm.InfraStructure.Data;
using Microsoft.EntityFrameworkCore;
using WebApiEcomm.InfraStructure.Repositores.UnitOfWork;
using WebApiEcomm.InfraStructure.Repositories;
using WebApiEcomm.Core.Services;
using WebApiEcomm.InfraStructure.Repositores.Service;
using Microsoft.Extensions.FileProviders;

namespace WebApiEcomm.InfraStructure
{
    public static class InfraStructureRegisteration
    {
        public static IServiceCollection InfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(GenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IImageManagementService, ImageManagementService>();
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}
