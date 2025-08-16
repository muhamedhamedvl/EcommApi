using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;
using WebApiEcomm.Core.Interfaces;
using WebApiEcomm.InfraStructure.Data;
using WebApiEcomm.InfraStructure.Repositories;
using AutoMapper;
using WebApiEcomm.Core.Services;
using StackExchange.Redis;
using WebApiEcomm.Core.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;
using WebApiEcomm.Core.Entites.Identity;

namespace WebApiEcomm.InfraStructure.Repositores.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;
        private readonly IConnectionMultiplexer redis;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;  
        private readonly IGenrateToken token;

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IPhotoRepository PhotoRepository { get; }

        public ICustomerBasketRepository CustomerBasketRepository { get; }

        public IAuth Auth { get; }

        public UnitOfWork(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService
             , IConnectionMultiplexer redis, UserManager<AppUser> userManager, IEmailService emailService, 
            SignInManager<AppUser> signInManager, IGenrateToken token)
        {
            this._context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
            this.redis = redis;
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.token = token;
            CategoryRepository = new CategoryRepository(_context);
            ProductRepository = new ProductRepository(_context, mapper, imageManagementService);
            PhotoRepository = new PhotoRepository(_context);
            CustomerBasketRepository = new CustomerBasketRepository(redis);
            Auth = new AuthRepository(userManager, emailService, signInManager ,token);

        }
    }
}
