using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using TP.NA.CartService.Application.Abstractions.Command;
using TP.NA.CartService.Application.Abstractions.Handlers;
using TP.NA.CartService.Application.Abstractions.Repository;
using TP.NA.CartService.Application.Commands.Cart.Request;
using TP.NA.CartService.Application.Commands.Cart.Response;
using TP.NA.CartService.Application.Commons;
using TP.NA.CartService.Application.Models;
using TP.NA.CartService.Domain.Entities;

namespace TP.NA.CartService.Application.Commands.Cart;

/// <summary>
/// Create cart command
/// </summary>
public class CreateCartCommand
{
    #region Result

    public class Result
    {
        public CreateCartResponse CreateCartResponse { get; set; } = new CreateCartResponse();
    }

    #endregion Result

    #region Command

    /// <summary>
    /// Command
    /// </summary>
    public class Command : CommonCommand, ICommand<Response<Result>>
    {
        public CreateCartRequest Request { get; set; }

        public Command()
        {
            Request = new CreateCartRequest();
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
                .ForMember(dest => dest.Products, act => act.MapFrom(src => src.Request.Products))
                .ReverseMap();

            CreateMap<Command, CartModel>()
                .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.Request.ClientId))
                .ForMember(dest => dest.Products, act => act.MapFrom(src => src.Request.Products))
                .ReverseMap();
        }
    }

    #endregion Mapper

    #region Validations

    /// <summary>
    /// Validate command create cart
    /// </summary>
    public class Validation : AbstractValidator<Command>
    {
        public Validation()
        {
            RuleFor(c => c.Request).NotNull();
            RuleFor(c => c.Request.ClientId).NotEmpty();
            RuleFor(c => c.Request.Products).NotNull();
            RuleFor(c => c.Request.Products).NotEmpty();
            RuleFor(c => c.Request.Products.FirstOrDefault().Id).NotEmpty();
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
            try
            {
                var map = _mapper.Map<Command, CartEntity>(request);

                var result = await _cartRepository.AddAsync(map);
                var model = _mapper.Map<CartEntity, CartModel>(result);

                _response.Payload.CreateCartResponse.Cart = model;
            }
            catch (Exception)
            {
                _response.SetFailureResponse("Get all carts", $"An error was throw trying to get all the carts");
                _response.Payload.CreateCartResponse.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return _response;
        }
    }

    #endregion Handler
}