namespace WebApiEcomm.API.Helper
{
    public class ResponseApi
    {
        public ResponseApi(int StatusCode , string Message = null)
        {
            this.StatusCode = StatusCode;
            this.Message = Message ?? GetMessage(StatusCode);
        }
        private string GetMessage(int StatusCode)
        {
            return StatusCode switch
            {
                200 => "Success",
                201 => "Created",
                204 => "No Content",
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => "Unknown Status"
            };
        }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
