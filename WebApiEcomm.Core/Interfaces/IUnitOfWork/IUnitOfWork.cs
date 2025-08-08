using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiEcomm.Core.Interfaces.Auth;

namespace WebApiEcomm.Core.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IPhotoRepository PhotoRepository { get; }
        public ICustomerBasketRepository CustomerBasketRepository { get; }
        public IAuth Auth { get; }
    }
}
