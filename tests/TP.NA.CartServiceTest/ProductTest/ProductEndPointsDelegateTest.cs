using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using TP.NA.CartService.Application.Abstractions.Repository;
using TP.NA.CartService.Application.Commands.Product;
using TP.NA.CartService.Application.Commons;
using TP.NA.CartService.Application.Models;
using TP.NA.CartService.Domain.Entities;
using TP.NA.CartServiceTest.MapperConfig;

namespace TP.NA.CartServiceTest.ProductTest
{
    public class ProductEndPointsDelegateTest
    {
        private Mock<ICartRepository> cartRepository;
        private Mock<IMediator> mediatorMock;
        private Mapper mapper;

        public ProductEndPointsDelegateTest()
        {
            cartRepository = CommonFactory.CartRepository;
            mapper = CommonFactory.GetMapper(AutoMapperConf.Configuration());
            mediatorMock = CommonFactory.GetMediatorMock;
        }

        #region Add product to cart

        [Theory]
        [InlineData("4")]
        public async Task AddProductToCartOkTest(string clientId)
        {
            //Arrange

            var command = ProductFactory.AddProductCommand;

            command.Request.Product = CartMockData.GetCartModels().FirstOrDefault().Products.FirstOrDefault();
            command.Request.Product.Id = Random.Shared.Next().ToString();
            command.Request.ClientId = clientId;
            var mapperMock = CommonFactory.GetMapperMock();
            var validationMock = CommonFactory.GetValidatorMock(ProductFactory.AddProductCommand);
            var cartEntities = CartMockData.GetCartEntities();
            var cartEntity = cartEntities.FirstOrDefault();
            cartEntity.UserId = clientId;

            var product = mapper.Map<ProductEntity>(command.Request.Product);

            var cartModel = mapper.Map<CartModel>(cartEntity);
            var response = new Response<AddProductCartCommand.Result>();
            response.StatusCode = StatusCodes.Status200OK;
            response.Payload = new AddProductCartCommand.Result();
            response.Payload.AddProductCartResponse.Cart = cartModel;

            mediatorMock.Setup(m => m.Send(It.IsAny<AddProductCartCommand.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable("Send");

            cartRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(cartEntities);

            cartRepository.Setup(m => m.AddProductAsync(It.IsAny<string>(), It.IsAny<ProductEntity>()))
                .ReturnsAsync(mapper.Map<CartEntity>(cartEntity));

            mapperMock.Setup(x => x.Map<AddProductCartCommand.Command, CartModel>(It.IsAny<AddProductCartCommand.Command>()))
                .Returns(cartModel);

            validationMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var handler = ProductFactory.AddProductCommandHandler(mapper, cartRepository.Object);

            //act

            var delegateResult = await ProductFactory.GetAddProductEndPoint.AddProductCart(mediatorMock.Object, command);
            var handlerResult = await handler.Handle(command, default);
            var validationResult = await validationMock.Object.ValidateAsync(command);

            //Assert

            mediatorMock.Verify(x => x.Send(It.IsAny<AddProductCartCommand.Command>(), default), Times.Once);

            Assert.NotNull(handlerResult);
            Assert.NotNull(delegateResult);

            Assert.IsType<Ok<Response<AddProductCartCommand.Result>>>(delegateResult);
            Assert.IsType<Response<AddProductCartCommand.Result>>(handlerResult);

            Assert.NotNull(handlerResult?.Payload?.AddProductCartResponse.Cart);

            Assert.IsType<CartModel>(handlerResult.Payload.AddProductCartResponse.Cart);

            Assert.Equal(clientId, handlerResult?.Payload?.AddProductCartResponse.Cart.UserId);

            Assert.True(validationResult?.IsValid);
            Assert.Equal(0, validationResult?.Errors.Count);

            ResetSetups();
        }

        [Theory]
        [InlineData("99", "1")]
        public async Task AddProductToCartBadRequestTest(string clientId, string productId)
        {
            //Arrange

            var command = ProductFactory.AddProductCommand;

            command.Request.Product = CartMockData.GetCartModels().FirstOrDefault().Products.FirstOrDefault();
            command.Request.Product.Id = productId;
            command.Request.ClientId = clientId;
            var mapperMock = CommonFactory.GetMapperMock();
            var validationMock = CommonFactory.GetValidatorMock(ProductFactory.AddProductCommand);
            var cartEntities = CartMockData.GetCartEntities();
            var cartEntity = cartEntities.FirstOrDefault();
            cartEntity.UserId = clientId;

            var product = mapper.Map<ProductEntity>(command.Request.Product);

            var cartModel = mapper.Map<CartModel>(cartEntity);
            var response = new Response<AddProductCartCommand.Result>();
            response.SetFailureResponse("Add product to cart", $"The product already exist in the cart");

            mediatorMock.Setup(m => m.Send(It.IsAny<AddProductCartCommand.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable("Send");

            cartRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(cartEntities);

            mapperMock.Setup(x => x.Map<AddProductCartCommand.Command, CartModel>(It.IsAny<AddProductCartCommand.Command>()))
                .Returns(cartModel);

            validationMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var handler = ProductFactory.AddProductCommandHandler(mapper, cartRepository.Object);

            //act

            var delegateResult = await ProductFactory.GetAddProductEndPoint.AddProductCart(mediatorMock.Object, command);
            var handlerResult = await handler.Handle(command, default);
            var validationResult = await validationMock.Object.ValidateAsync(command);

            //Assert

            mediatorMock.Verify(x => x.Send(It.IsAny<AddProductCartCommand.Command>(), default), Times.Once);

            Assert.NotNull(handlerResult);
            Assert.NotNull(delegateResult);

            Assert.IsType<BadRequest<Response<AddProductCartCommand.Result>>>(delegateResult);
            Assert.IsType<Response<AddProductCartCommand.Result>>(handlerResult);

            Assert.Equal(StatusCodes.Status400BadRequest, handlerResult.StatusCode);
            Assert.True(handlerResult.Errors.Count >= 1);

            Assert.Equal(true, validationResult?.IsValid);
            Assert.Equal(0, validationResult?.Errors.Count);

            ResetSetups();
        }

        [Theory]
        [InlineData("0", "1")]
        public async Task AddProductToCartUnprocessableEntityTest(string clientId, string productId)
        {
            //Arrange

            var command = ProductFactory.AddProductCommand;

            command.Request.Product = CartMockData.GetCartModels().FirstOrDefault().Products.FirstOrDefault();
            command.Request.Product.Id = productId;
            command.Request.ClientId = clientId;
            var validationMock = CommonFactory.GetValidatorMock(ProductFactory.AddProductCommand);

            var response = new Response<AddProductCartCommand.Result>();
            response.SetFailureResponse("Add product to cart", $"The product already exist in the cart");

            var validationResponse = new FluentValidation.Results.ValidationResult()
            {
                Errors = new List<FluentValidation.Results.ValidationFailure>()
                {
                    new FluentValidation.Results.ValidationFailure()
                    {
                        ErrorMessage = "Bad entity"
                    }
                }
            };

            validationMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(validationResponse);

            //act
            var validationResult = await validationMock.Object.ValidateAsync(command);

            //Assert

            Assert.Equal(false, validationResult?.IsValid);
            Assert.Equal(1, validationResult?.Errors.Count);

            ResetSetups();
        }

        #endregion Add product to cart

        #region Delete product

        [Theory]
        [InlineData("4", "1")]
        public async Task DeleteProductFromCartOkTest(string clientId, string productId)
        {
            //Arrange

            var command = ProductFactory.DeleteProductCommand;
            command.Request.ProductId = productId;
            command.Request.ClientId = clientId;

            var mapperMock = CommonFactory.GetMapperMock();
            var validationMock = CommonFactory.GetValidatorMock(ProductFactory.DeleteProductCommand);
            var cartEntities = CartMockData.GetCartEntities();
            var cartEntity = cartEntities.FirstOrDefault();
            cartEntity.UserId = clientId;

            var productsCount = cartEntity.Products.Count;


            var cartModel = mapper.Map<CartModel>(cartEntity);
            cartModel.Products.Remove(cartModel.Products.FirstOrDefault(c => c.Id == productId));

            var response = new Response<DeleteProductCartCommand.Result>();
            response.StatusCode = StatusCodes.Status200OK;
            response.Payload = new DeleteProductCartCommand.Result();
            response.Payload.DeleteProductCartResponse.Cart = cartModel;

            mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCartCommand.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable("Send");

            cartRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(cartEntities);

            cartRepository.Setup(m => m.DeleteProductAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(mapper.Map<CartEntity>(cartEntity));

            mapperMock.Setup(x => x.Map<DeleteProductCartCommand.Command, CartModel>(It.IsAny<DeleteProductCartCommand.Command>()))
                .Returns(cartModel);

            validationMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var handler = ProductFactory.DeleteProductCommandHandler(mapper, cartRepository.Object);

            //act

            var delegateResult = await ProductFactory.GetDeleteProductDelegate.RemoveProductCart(mediatorMock.Object, command);
            var handlerResult = await handler.Handle(command, default);
            var validationResult = await validationMock.Object.ValidateAsync(command);


            //Assert

            mediatorMock.Verify(x => x.Send(It.IsAny<DeleteProductCartCommand.Command>(), default), Times.Once);

            Assert.NotNull(handlerResult);
            Assert.NotNull(delegateResult);

            Assert.IsType<Ok<Response<DeleteProductCartCommand.Result>>>(delegateResult);
            Assert.IsType<Response<DeleteProductCartCommand.Result>>(handlerResult);

            Assert.NotNull(handlerResult?.Payload?.DeleteProductCartResponse.Cart);

            Assert.IsType<CartModel>(handlerResult.Payload.DeleteProductCartResponse.Cart);

            handlerResult.Payload.DeleteProductCartResponse.Cart = cartModel;

            Assert.Equal(productsCount - 1, handlerResult.Payload.DeleteProductCartResponse.Cart.Products.Count);

            var productDeleted = handlerResult?.Payload?.DeleteProductCartResponse.Cart.Products.FirstOrDefault(c => c.Id == productId);

            Assert.Null(productDeleted);

            Assert.Equal(clientId, handlerResult?.Payload?.DeleteProductCartResponse.Cart.UserId);

            Assert.Equal(true, validationResult?.IsValid);
            Assert.Equal(0, validationResult?.Errors.Count);

            ResetSetups();
        }

        [Theory]
        [InlineData("1", "99")]
        [InlineData("1", "104")]
        [InlineData("88", "1")]
        [InlineData("70", "1")]
        public async Task DeleteProductFromCartBadClientAndProductTest(string clientId, string productId)
        {
            //arrange

            var command = ProductFactory.DeleteProductCommand;
            command.Request.ProductId = productId;
            command.Request.ClientId = clientId;

            var mapperMock = CommonFactory.GetMapperMock();
            var validationMock = CommonFactory.GetValidatorMock(ProductFactory.DeleteProductCommand);
            var cartEntities = CartMockData.GetCartEntities();

            var response = new Response<DeleteProductCartCommand.Result>();
            response.SetFailureResponse("Cart", "Cart do not exists");

            mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCartCommand.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable("Send");

            cartRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(cartEntities);

            validationMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            var handler = ProductFactory.DeleteProductCommandHandler(mapper, cartRepository.Object);

            //act

            var delegateResult = await ProductFactory.GetDeleteProductDelegate.RemoveProductCart(mediatorMock.Object, command);
            var handlerResult = await handler.Handle(command, default);
            var validationResult = await validationMock.Object.ValidateAsync(command);

            //Assert

            mediatorMock.Verify(x => x.Send(It.IsAny<DeleteProductCartCommand.Command>(), default), Times.Once);

            Assert.NotNull(handlerResult);
            Assert.NotNull(delegateResult);

            Assert.IsType<BadRequest<Response<DeleteProductCartCommand.Result>>>(delegateResult);
            Assert.IsType<Response<DeleteProductCartCommand.Result>>(handlerResult);

            Assert.Null(handlerResult?.Payload?.DeleteProductCartResponse.Cart);

            Assert.Equal(true, validationResult?.IsValid);
            Assert.Equal(0, validationResult?.Errors.Count);

            ResetSetups();
        }

        #endregion Delete product

        private void ResetSetups()
        {
            mediatorMock.Reset();
            cartRepository.Reset();
        }
    }
}