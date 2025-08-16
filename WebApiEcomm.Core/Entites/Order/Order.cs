using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiEcomm.Core.Entites.Order
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }
        public Order(string buyerEmail, decimal subTotal, ShippingAddress shippingaddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> orderItems)
        {
            BuyerEmail = buyerEmail;
            SubTotal = subTotal;
            this.shippingaddress = shippingaddress;
            this.deliveryMethod = deliveryMethod;
            this.OrderItems = orderItems;
        }

        public string BuyerEmail { get; set; }
        public decimal SubTotal { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public ShippingAddress shippingaddress { get; set; }
        public DeliveryMethod deliveryMethod { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Status status { get; set; } = Status.Pending;
        public decimal GetTotalOrder()
        {
            return SubTotal + deliveryMethod.Price;
        }
    }
}
