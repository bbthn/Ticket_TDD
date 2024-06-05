using Microsoft.AspNetCore.Http;

namespace WebApi.Middlewares
{
    public class ErrorResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
