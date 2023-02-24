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
    /// Get all carts end point
    /// </summary>
    public class GetAllCartsEndPoint : ICarterModule
    {
        public async Task<IResult> getAllCarts(IMediator mediator)
        {
            var result = await mediator.Send(new GetAllCartsQuery.Query());
            return Results.Ok(result);
        }

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/cartService", getAllCarts)
            .WithDisplayName("Cart")
            .Produces<Response<GetAllCartsQuery.Result>>(StatusCodes.Status200OK);
        }
    }
}