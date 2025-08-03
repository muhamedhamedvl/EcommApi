using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEcomm.Core.Sharing
{
    public class ProductParams
    {
        //  string sort , int? CategoryId , int PageSize , int PageNumber
        public string sort {  get; set; }

        public int? CategoryId {  get; set; }

        public int MaxPageSize { get; set; } = 6;

        private int _PageSize = 3;

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        public int PageNumber { get; set; } = 1;

    }
}
