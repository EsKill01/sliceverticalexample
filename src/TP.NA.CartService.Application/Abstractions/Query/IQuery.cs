using MediatR;

namespace TP.NA.CartService.Application.Abstractions.Query
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}