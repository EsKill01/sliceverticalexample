namespace TP.NA.CartService.Application.Abstractions.Handlers
{
    using CartService.Application.Abstractions.Query;
    using MediatR;

    /// <summary>
    /// Query handler interface
    /// </summary>
    /// <typeparam name="TQuery">TQuery</typeparam>
    /// <typeparam name="TResponse">TResponse</typeparam>
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }
}