using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TP.NA.CartService.Application.Commands.Product;
using TP.NA.CartService.Application.Commons;
using TP.NA.CartService.Application.Config;

namespace TP.NA.CartService.Application.EndPoints.Product
{
    public class AddProductCartEndPoint : ICarterModule
    {
        public async Task<IResult> AddProductCart(IMediator mediator, [FromBody] AddProductCartCommand.Command request)
        {
            var result = await mediator.Send(request);

            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/cartService/addProduct", AddProductCart)
                 .WithDisplayName("Product")
                 .Accepts<AddProductCartCommand.Command>("application/json")
                 .Produces<Response<AddProductCartCommand.Result>>(StatusCodes.Status200OK)
                 .Produces<IResponse>(StatusCodes.Status400BadRequest)
                 .Produces<IResponse>(StatusCodes.Status422UnprocessableEntity)
                 .AddEndpointFilter<ValidationFilter<
                     AddProductCartCommand.Command,
                     AddProductCartCommand.Result>>();
        }
    }
}