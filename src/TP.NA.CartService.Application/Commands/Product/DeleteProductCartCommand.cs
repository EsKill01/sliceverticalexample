using AutoMapper;
using FluentValidation;
using TP.NA.CartService.Application.Abstractions.Command;
using TP.NA.CartService.Application.Abstractions.Handlers;
using TP.NA.CartService.Application.Abstractions.Repository;
using TP.NA.CartService.Application.Commands.Product.Request;
using TP.NA.CartService.Application.Commands.Product.Response;
using TP.NA.CartService.Application.Commons;
using TP.NA.CartService.Application.Exceptions;
using TP.NA.CartService.Application.Models;
using TP.NA.CartService.Domain.Entities;

namespace TP.NA.CartService.Application.Commands.Product
{
    public class DeleteProductCartCommand
    {
        #region Result

        public class Result
        {
            public DeleteProductCartResponse DeleteProductCartResponse { get; set; } = new DeleteProductCartResponse();
        }

        #endregion Result

        #region Command

        public class Command : CommonCommand, ICommand<Response<Result>>
        {
            public DeleteProductCartRequest Request { get; set; }

            public Command()
            {
                Request = new DeleteProductCartRequest();
            }
        }

        #endregion Command

        #region Validations

        public class Validation : AbstractValidator<Command>
        {
            public Validation()
            {
                RuleFor(c => int.Parse(c.Request.ProductId)).GreaterThan(0);
                RuleFor(c => int.Parse(c.Request.ClientId)).GreaterThan(0);
            }
        }

        #endregion Validations

        #region Handler

        public class Handler : ICommandHandler<Command, Response<Result>>
        {
            private readonly IMapper _mapper;

            private readonly ICartRepository _cartRepository;

            private readonly Response<Result> _response;

            public Handler(IMapper mapper, ICartRepository cartRepository)
            {
                _mapper = mapper;
                _cartRepository = cartRepository;
                _response = new Response<Result>
                {
                    Payload = new Result()
                };
            }

            public async Task<Response<Result>> Handle(Command request, CancellationToken cancellationToken)
            {
                var carts = await _cartRepository.GetAllAsync();

                var cart = carts?.FirstOrDefault(c => c.UserId == request.Request.ClientId && c.Active);

                if (cart == null)
                {
                    _response.SetFailureResponse("Cart", "Cart does not exists");
                    return _response;
                }

                var product = cart?.Products.FirstOrDefault(c => c.Id == request.Request.ProductId);

                if (product == null)
                {
                    _response.SetFailureResponse("Product", "Product does not exists in the cart");
                    return _response;
                }

                var response = await _cartRepository.DeleteProductAsync(cart.Id, product.Id);

                var model = _mapper.Map<CartEntity, CartModel>(response);

                _response.Payload.DeleteProductCartResponse.Cart = model;

                return _response;
            }
        }

        #endregion Handler
    }
}