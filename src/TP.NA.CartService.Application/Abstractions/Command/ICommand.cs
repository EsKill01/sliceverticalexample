namespace TP.NA.CartService.Application.Abstractions.Command
{
    using MediatR;

    /// <summary>
    /// Command interface
    /// </summary>
    /// <typeparam name="IResponse">Response interface</typeparam>
    public interface ICommand<out IResponse> : IRequest<IResponse>
    {
    }
}