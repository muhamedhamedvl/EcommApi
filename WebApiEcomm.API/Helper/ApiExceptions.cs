namespace WebApiEcomm.API.Helper
{
    public class ApiExceptions : ResponseApi
    {
        public ApiExceptions(int statusCode, string message, string details = "")
            : base(statusCode, message)
        {
            Details = details;
        }

        public string Details { get; set; } = string.Empty;
    }
}
