namespace TP.NA.CartService.Application.Config
{
    using FluentValidation;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using TP.NA.CartService.Application.Commons;

    /// <summary>
    /// Validation filter
    /// </summary>
    /// <typeparam name="T">Command or query</typeparam>
    public class ValidationFilter<T, Y> : IEndpointFilter
    {
        private readonly ILogger _logger;

        public ValidationFilter(ILogger<ValidationFilter<T, Y>> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            Response<Y> result = new Response<Y>();
            List<ValidationMessage> errorsMessages = new List<ValidationMessage>();
            T? argoToValidate = context.GetArgument<T>(1);

            IValidator<T> validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

            if (validator is not null)
            {
                try
                {
                    var validationResult = await validator.ValidateAsync(argoToValidate!);
                    if (!validationResult.IsValid)
                    {
                        var errors = validationResult.Errors.ToList();

                        foreach (var item in errors)
                        {
                            errorsMessages.Add(new ValidationMessage
                            {
                                Message = item.ErrorMessage,
                                Property = item.PropertyName
                            });
                        }

                        result.SetFailureResponse(errorsMessages, statusCode: StatusCodes.Status422UnprocessableEntity);

                        return Results.UnprocessableEntity(result);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Something went wrong: {ex}");
                    result.SetFailureResponse("Model", "Invalid model", StatusCodes.Status422UnprocessableEntity);
                    return Results.UnprocessableEntity(result);
                }
            }

            return await next.Invoke(context);
        }
    }
}