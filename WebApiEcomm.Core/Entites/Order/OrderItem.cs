namespace WebApiEcomm.Core.Entites.Order
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(int productId, string productName, decimal price, int quantity, string image)
        {
            ProductId = productId;
            Image = image;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}