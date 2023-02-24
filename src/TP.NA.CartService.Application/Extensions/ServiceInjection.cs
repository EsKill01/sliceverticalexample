using Carter;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using TP.NA.CartService.Application.Abstractions.Logger;
using TP.NA.CartService.Application.Commands.Cart;
using TP.NA.CartService.Application.Commands.Product;
using TP.NA.CartService.Application.Config;

namespace TP.NA.CartService.Application.Extensions
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Use this if you want to use validation exception behavior
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddLogging();

            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestLogger<>));
            services.AddTransient<IValidator<CreateCartCommand.Command>, CreateCartCommand.Validation>();
            services.AddTransient<IValidator<AddProductCartCommand.Command>, AddProductCartCommand.Validation>();
            services.AddTransient<IValidator<DeleteProductCartCommand.Command>, DeleteProductCartCommand.Validation>();

            services.AddCarter();
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }

        /// <summary>
        /// Method to configure application
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigApp(this IServiceCollection builder)
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            builder.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}