using AutoMapper;
using TP.NA.CartService.Application.Abstractions.Repository;
using TP.NA.CartService.Application.Commands.Product;
using TP.NA.CartService.Application.EndPoints.Product;
using TP.NA.CartService.Application.Models;
using TP.NA.CartService.Domain.Entities;

namespace TP.NA.CartServiceTest.ProductTest
{
    /// <summary>
    /// Product factory for testing
    /// </summary>
    public class ProductFactory
    {
        #region handlers

        public static AddProductCartCommand.Handler AddProductCommandHandler(IMapper mapper, ICartRepository repository) => new AddProductCartCommand.Handler(mapper, repository);

        public static DeleteProductCartCommand.Handler DeleteProductCommandHandler(IMapper mapper, ICartRepository repository) => new DeleteProductCartCommand.Handler(mapper, repository);

        #endregion handlers

        #region commands

        public static AddProductCartCommand.Command AddProductCommand => new AddProductCartCommand.Command();

        public static DeleteProductCartCommand.Command DeleteProductCommand => new DeleteProductCartCommand.Command();

        #endregion commands

        #region objects

        public static ProductEntity GetProductEntity = new ProductEntity();

        public static ProductModel GetProductModel = new ProductModel();

        #endregion objects

        #region delegates

        public static AddProductCartEndPoint GetAddProductEndPoint = new AddProductCartEndPoint();

        public static DeleteProductCartEndPoint GetDeleteProductDelegate = new DeleteProductCartEndPoint();

        #endregion delegates
    }
}