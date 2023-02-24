using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using TP.NA.CartService.Application.Commands.Cart;
using TP.NA.CartService.Application.Commons;
using TP.NA.CartService.Application.Config;

namespace TP.NA.CartService.Application.EndPoints.Cart
{
    public class CreateCartEndPoint : ICarterModule
    {
        public async Task<IResult> CreateCart(IMediator mediator, [FromBody] CreateCartCommand.Command request)
        {
            var result = await mediator.Send(request);
            var route = new
            {
                cartId = result.Payload.CreateCartResponse.Cart.Id
            };

            return result.IsError ? Results.BadRequest(result) : Results.CreatedAtRoute("GetCartById", route);
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/v1/cartService/", CreateCart)
                .WithDisplayName("Cart")
                .Accepts<CreateCartCommand.Command>("application/json")
                .Produces(StatusCodes.Status201Created)
                .Produces<IResponse>(StatusCodes.Status400BadRequest)
                .Produces<IResponse>(StatusCodes.Status422UnprocessableEntity)
                ///Remove this above line and uncomment injection of validation exception RequestValidationBehavior
                ///if you want to use validation exception instead validation filter
                .AddEndpointFilter<ValidationFilter<CreateCartCommand.Command, CreateCartCommand.Result>>();
        }
    }
}