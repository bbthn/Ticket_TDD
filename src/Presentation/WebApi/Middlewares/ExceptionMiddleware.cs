using System.Net.Mime;
using System.Net;
using Newtonsoft.Json;

namespace WebApi.Middlewares
{
    public static class ExceptionMiddleware
    {
        public class ErrorHandlerMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<ErrorHandlerMiddleware> _logger;
            /// <summary>
            /// Global ExceptionHandler
            /// </summary>
            /// <param name="next"></param>
            /// <param name="logger"></param>
            public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }
            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    await HandleExceptionAsync(context, ex);
                }
            }
            private static Task HandleExceptionAsync(HttpContext context, Exception ex)
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                return context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResponseModel()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "Internal Server Error. Please try again later."
                }));
            }
        }

    }
}
