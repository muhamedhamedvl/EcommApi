using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;

namespace WebApiEcomm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork _work;
        protected readonly IMapper mapper;
        public BaseController(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            this.mapper = mapper;
        }
    }
}
