using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using TP.NA.CartService.Application.Commons;
using TP.NA.CartService.Application.Queries.Cart;

namespace TP.NA.CartService.Application.EndPoints.Cart
{
    /// <summary>
    /// Get cart by id end point
    /// </summary>
    public class GetCartByIdEndPoint : ICarterModule
    {
        public async Task<IResult> GetCartByIdDelegate(IMediator mediator, string cartId)
        {
            var query = new GetCartByIdQuery.Query(cartId);

            var result = await mediator.Send(query);

            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/cartService/{cartId}", GetCartByIdDelegate)
           .WithDisplayName("Cart")
           .WithName("GetCartById")
           .Produces<Response<GetCartByIdQuery.Result>>(StatusCodes.Status200OK)
           .Produces<IResponse>(StatusCodes.Status404NotFound)
           .Produces<IResponse>(StatusCodes.Status400BadRequest);
        }
    }
}