namespace WebApiEcomm.Core.Entites.Order
{
    public class DeliveryMethod : BaseEntity
    {
        public DeliveryMethod()
        {
            
        }
        public DeliveryMethod(string name, string deliveryTime, string description, decimal price)
        {
            Name = name;
            DeliveryTime = deliveryTime;
            Description = description;
            Price = price;
        }

        public string Name { get; set; }
        public string DeliveryTime { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }


    }
}
