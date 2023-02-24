namespace TP.NA.CartServiceTest.CartTest
{
    using AutoMapper;
    using CartService.Application.Abstractions.Repository;
    using CartService.Application.Commands.Cart;
    using CartService.Application.EndPoints.Cart;
    using CartService.Application.Models;
    using CartService.Application.Queries.Cart;
    using CartService.Domain.Entities;

    /// <summary>
    /// Cart factory for testing
    /// </summary>
    public class CartFactory
    {
        #region handlers

        public static CreateCartCommand.Handler CreateCartCommandHandler(IMapper mapper, ICartRepository repository) => new CreateCartCommand.Handler(mapper, repository);

        public static GetCartByIdQuery.Handler GetCartByIdHandler(IMapper mapper, ICartRepository repository) => new GetCartByIdQuery.Handler(mapper, repository);

        public static GetAllCartsQuery.Handler GetAllCartsHandler(IMapper mapper, ICartRepository repository) => new GetAllCartsQuery.Handler(mapper, repository);

        #endregion handlers

        #region commands

        public static CreateCartCommand.Command CartCommand => new CreateCartCommand.Command();

        #endregion commands

        #region queries

        public static GetCartByIdQuery.Query GetCartByIdQuery(string id) => new GetCartByIdQuery.Query(id);

        public static GetAllCartsQuery.Query GetAllCartQuery => new GetAllCartsQuery.Query();

        #endregion queries

        #region objects

        public static CartEntity GetCartEntity = new CartEntity();

        public static CartModel GetCartModel = new CartModel();

        #endregion objects

        #region delegates

        public static GetAllCartsEndPoint GetAllCartDelegate = new GetAllCartsEndPoint();

        public static GetCartByIdEndPoint GetCartByIdDelegate = new GetCartByIdEndPoint();

        public static CreateCartEndPoint CreateCartDelegate = new CreateCartEndPoint();

        #endregion delegates
    }
}