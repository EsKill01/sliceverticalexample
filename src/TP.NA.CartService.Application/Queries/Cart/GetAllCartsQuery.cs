namespace TP.NA.CartService.Application.Queries.Cart
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using TP.NA.CartService.Application.Abstractions.Handlers;
    using TP.NA.CartService.Application.Abstractions.Query;
    using TP.NA.CartService.Application.Abstractions.Repository;
    using TP.NA.CartService.Application.Commons;
    using TP.NA.CartService.Application.Models;
    using TP.NA.CartService.Application.Queries.Cart.Request;
    using TP.NA.CartService.Application.Queries.Cart.Response;

    /// <summary>
    /// Get all carts query
    /// </summary>
    public class GetAllCartsQuery
    {
        #region Result

        /// <summary>
        /// Get all carts result
        /// </summary>
        public class Result
        {
            /// <summary>
            /// Gets or sets get all carts response
            /// </summary>
            public GetAllCartsResponse GetAllCartsReponse { get; set; }

            /// <summary>
            /// Create a all carts response
            /// </summary>
            public Result()
            {
                GetAllCartsReponse = new GetAllCartsResponse();
            }
        }

        #endregion Result

        #region Query

        /// <summary>
        /// Get all carts query
        /// </summary>
        public class Query : IQuery<Response<Result>>
        {
            /// <summary>
            /// Get all carts request
            /// </summary>
            public GetAllCartsRequest Request { get; set; } = new GetAllCartsRequest();
        }

        #endregion Query

        #region Handler

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
                try
                {
                    var result = await _cartRepository.GetAllAsync();

                    _response.Payload.GetAllCartsReponse.Carts = _mapper.Map<IEnumerable<CartModel>>(result);
                }
                catch (Exception ex)
                {
                    _response.SetFailureResponse("Get all carts", $"An error was throw trying to get all the carts");
                    _response.Payload.GetAllCartsReponse.StatusCode = StatusCodes.Status500InternalServerError;
                }

                return _response;
            }
        }

        #endregion Handler
    }
}