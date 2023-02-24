using AutoMapper;
using TP.NA.CartService.Application.Abstractions.Handlers;
using TP.NA.CartService.Application.Abstractions.Query;
using TP.NA.CartService.Application.Abstractions.Repository;
using TP.NA.CartService.Application.Commons;
using TP.NA.CartService.Application.Exceptions;
using TP.NA.CartService.Application.Models;
using TP.NA.CartService.Application.Queries.Cart.Request;
using TP.NA.CartService.Application.Queries.Cart.Response;

namespace TP.NA.CartService.Application.Queries.Cart
{
    /// <summary>
    /// Get cart by id query
    /// </summary>
    public class GetCartByIdQuery
    {
        #region Result

        /// <summary>
        /// Get cart by id result
        /// </summary>
        public class Result
        {
            /// <summary>
            /// Gets or sets cart by id response
            /// </summary>
            public GetCartByIdResponse GetCartByIdResponse { get; set; }

            /// <summary>
            /// Create a new result
            /// </summary>
            public Result()
            {
                GetCartByIdResponse = new GetCartByIdResponse();
            }
        }

        #endregion Result

        #region Query

        /// <summary>
        /// Get cart by id query class
        /// </summary>
        public class Query : IQuery<Response<Result>>
        {
            /// <summary>
            /// Gets or sets get cart by id request
            /// </summary>
            public GetCartByIdRequest Request { get; set; } = new GetCartByIdRequest();

            public Query(string id)
            {
                Request.Id = id;
            }
        }

        #endregion Query

        #region Handler

        /// <summary>
        /// Get cart by id handler
        /// </summary>
        public class Handler : IQueryHandler<Query, Response<Result>>
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

            public async Task<Response<Result>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _cartRepository.GetByIdAsync(request.Request.Id);

                if (result == null)
                {
                    throw new NotFoundException("Cart do not exits");
                }

                _response.Payload.GetCartByIdResponse.Cart = _mapper.Map<CartModel>(result);

                return _response;
            }
        }

        #endregion Handler
    }
}