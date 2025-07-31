namespace WebApiEcomm.API.Helper
{
    public class ApiExceptions : ResponseApi
    {
        public ApiExceptions(string message, string details, int statusCode = 500) : base(statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; } = string.Empty;
    }
}
