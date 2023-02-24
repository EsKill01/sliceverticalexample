using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using TP.NA.CartService.Application.Abstractions.Command;
using TP.NA.CartService.Application.Abstractions.Handlers;
using TP.NA.CartService.Application.Abstractions.Repository;
using TP.NA.CartService.Application.Commands.Product.Request;
using TP.NA.CartService.Application.Commands.Product.Response;
using TP.NA.CartService.Application.Commons;
using TP.NA.CartService.Application.Models;
using TP.NA.CartService.Domain.Entities;

namespace TP.NA.CartService.Application.Commands.Product
{
    public class AddProductCartCommand
    {
        #region Result

        public class Result
        {
            public AddProductCartResponse AddProductCartResponse { get; set; } = new AddProductCartResponse();
        }

        #endregion Result

        #region Command

        public class Command : CommonCommand, ICommand<Response<Result>>
        {
            public AddProductCartRequest Request { get; set; }

            public Command()
            {
                Request = new AddProductCartRequest();
            }
        }

        #endregion Command

        #region Mapper

        public class Mapper : Profile
        {
            public Mapper()
            {
                CreateMap<Command, CartEntity>()
                    .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.Request.ClientId))
                    .ReverseMap();
            }
        }

        #endregion Mapper

        #region Validations

        public class Validation : AbstractValidator<Command>
        {
            public Validation()
            {
                RuleFor(c => c.Request).NotNull();
                RuleFor(c => c.Request).NotEmpty();
                RuleFor(c => int.Parse(c.Request.ClientId)).NotEmpty().GreaterThan(0);
                RuleFor(c => c.Request.Product).NotNull();
                RuleFor(c => int.Parse(c.Request.Product.Id)).GreaterThan(0);
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
                var cart = carts.FirstOrDefault(c => c.UserId == request.Request.ClientId && c.Active);

                if (cart != null)
                {
                    var alreadyProductAdded = cart.Products.FirstOrDefault(c => c.Id == request.Request.Product.Id);

                    if (alreadyProductAdded != null)
                    {
                        _response.SetFailureResponse("Add product to cart", $"The product already exist in the cart");
                        _response.StatusCode = StatusCodes.Status400BadRequest;

                        return _response;
                    }


                    var map = _mapper.Map<ProductModel, ProductEntity>(request.Request.Product);

                    var response = await _cartRepository.AddProductAsync(cart.Id, map);

                    var model = _mapper.Map<CartEntity, CartModel>(response);

                    _response.Payload.AddProductCartResponse.Cart = model;

                    return _response;
                }
                else
                {
                    _response.SetFailureResponse("Add product to cart", $"The cart do not exists");
                    _response.StatusCode = StatusCodes.Status400BadRequest;

                    return _response;
                }


            }
        }

        #endregion Handler
    }
}