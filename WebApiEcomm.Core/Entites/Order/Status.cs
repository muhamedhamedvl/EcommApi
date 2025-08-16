namespace WebApiEcomm.Core.Entites.Order
{
    public enum Status
    {
        Pending,
        PaymentReceived,
        PaymentFailed,
        Shipped,
        Delivered,
        Cancelled
    }
}