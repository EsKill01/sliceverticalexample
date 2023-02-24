using Microsoft.AspNetCore.Http;

namespace TP.NA.CartService.Application.Abstractions.ExceptionMiddleware
{
    public interface IExceptionMiddleware
    {
        Task InvokeAsync(HttpContext httpContext);

        Task HandleExceptionAsync(HttpContext context, Exception exception);
    }
}