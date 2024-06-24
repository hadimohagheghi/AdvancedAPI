using Common;
using Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebFramework.Api;

namespace WebFramework.Middlewares
{
    public  static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }

    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next,ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"خطایی رخ داده است");
                
                var apiResult = new ApiResult(false,ApiResultStatusCode.ServerError);
                var json = JsonConvert.SerializeObject(apiResult);

                await context.Response.WriteAsync(json);
            }
        }
    }
}
