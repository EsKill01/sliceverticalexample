namespace TP.NA.CartService.Application.Abstractions.Handlers
{
    using MediatR;
    using TP.NA.CartService.Application.Abstractions.Command;

    internal interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
    }
}