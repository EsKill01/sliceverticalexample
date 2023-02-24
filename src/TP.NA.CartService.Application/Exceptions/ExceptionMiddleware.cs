namespace TP.NA.CartService.Application.Exceptions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Text.Json;
    using TP.NA.CartService.Application.Abstractions.ExceptionMiddleware;
    using TP.NA.CartService.Application.Abstractions.Logger;
    using TP.NA.CartService.Application.Commons;

    /// <summary>
    /// Exception middle ware class
    /// </summary>
    public class ExceptionMiddleware : IExceptionMiddleware
    {
        /// <summary>
        /// Request delegate
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Logger interface
        /// </summary>
        private readonly ILogger _logger;

        private readonly ILoggerManager _loggerManager;

        /// <summary>
        /// Validation Exception handler
        /// </summary>
        /// <param name="exception">Validation exception</param>
        /// <returns>Problems details and status code int</returns>
        private (ProblemDetails, int) HandleValidationException(ValidationException exception)
        {
            var details = new ValidationProblemDetails(exception.failures)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            return (details, StatusCodes.Status422UnprocessableEntity);
        }

        /// <summary>
        /// Not found Exception handler
        /// </summary>
        /// <param name="exception">Not found exception</param>
        /// <returns>Problems details and status code int</returns>
        private (ProblemDetails, int) HandleNotFoundException(NotFoundException exception)
        {
            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            };

            return (details, StatusCodes.Status404NotFound);
        }

        /// <summary>
        /// Forbidden access exception handler
        /// </summary>
        /// <param name="exception">Forbidden access exception</param>
        /// <returns>Problems details and status code int</returns>
        private (ProblemDetails, int) HandleForbiddenAccessException(ForbiddenAccessException exception)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                Detail = exception.Message
            };

            return (details, StatusCodes.Status403Forbidden);
        }

        /// <summary>
        ///
        /// Default exception handler
        /// </summary>
        /// <param name="exception">Default exception</param>
        /// <returns>Problems details and status code int</returns>
        private (ProblemDetails, int) HandleUnknownException(Exception exception)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = exception.Message
            };

            return (details, StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Request delegate for exception middle ware
        /// </summary>
        /// <param name="next">Request delegate next</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, ILoggerManager loggerManager)
        {
            _loggerManager = loggerManager;
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// Invoke Async method
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                _loggerManager.LogError($"Something went wrong: {ex}");

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="context">Current context</param>
        /// <param name="exception">Current Exception</param>
        /// <returns>A task</returns>
        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ProblemDetails details = null;
            int statusCode = 0;
            string property = string.Empty;

            switch (exception)
            {
                case ValidationException validationEx:
                    (details, statusCode) = HandleValidationException(validationEx);
                    property = nameof(ValidationException);
                    break;

                case NotFoundException notFoundEx:
                    (details, statusCode) = HandleNotFoundException(notFoundEx);
                    property = nameof(NotFoundException);
                    break;

                case ForbiddenAccessException:
                    (details, statusCode) = HandleForbiddenAccessException(exception as ForbiddenAccessException);
                    property = nameof(ForbiddenAccessException);
                    break;

                default:
                    (details, statusCode) = HandleUnknownException(exception);
                    break;
            }

            var message = new ValidationMessage
            {
                Message = exception.Message,
                Property = string.IsNullOrEmpty(property) ? "Exception" : property,
            };

            ///Uncomment this if you want return exception details to the user in the 'payload' object
            ////var apiResponse = new Response<ProblemDetails>
            ////{
            ////  IsError = true,
            ////  Errors = new List<ValidationMessage>(),
            ////  StatusCode = statusCode,
            ////  Payload = details
            ////};

            ///Comment this above if you want to use api Response
            var apiResponse = new BaseResponse
            {
                IsError = true,
                Errors = new List<ValidationMessage>(),
                StatusCode = statusCode
            };

            apiResponse.Errors.Add(message);

            var json = JsonSerializer.Serialize(apiResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(json);
        }
    }
}