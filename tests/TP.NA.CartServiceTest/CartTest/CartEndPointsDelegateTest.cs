using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using System.Text;
using System.Text.Json;
using TP.NA.CartService.Application.Abstractions.ExceptionMiddleware;
using TP.NA.CartService.Application.Abstractions.Handlers;
using TP.NA.CartService.Application.Abstractions.Repository;
using TP.NA.CartService.Application.Commands.Cart;
using TP.NA.CartService.Application.Commons;
using TP.NA.CartService.Application.Exceptions;
using TP.NA.CartService.Application.Models;
using TP.NA.CartService.Application.Queries.Cart;
using TP.NA.CartService.Application.Queries.Cart.Response;
using TP.NA.CartService.Domain.Entities;
using TP.NA.CartServiceTest.MapperConfig;
using static TP.NA.CartService.Application.Queries.Cart.GetCartByIdQuery;

namespace TP.NA.CartServiceTest.CartTest
{
    public class CartEndPointsDelegateTest
    {
        private Mock<IMediator> mediatorMock;
        private Mock<ICartRepository> cartRepository;
        private Mapper mapper;

        public CartEndPointsDelegateTest()
        {
            mediatorMock = CommonFactory.GetMediatorMock;
            cartRepository = CommonFactory.CartRepository;
            mapper = CommonFactory.GetMapper(AutoMapperConf.Configuration());
        }

        [Fact]
        public async Task GetAllCartsTest()
        {
            //arrange

            var reponse = new Response<GetAllCartsQuery.Result>()
            {
                Payload = new GetAllCartsQuery.Result()
                {
                    GetAllCartsReponse = new GetAllCartsResponse()
                    {
                        Carts = CartMockData.GetCartModels().ToArray()
                    }
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllCartsQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(await Task.FromResult(reponse))
                .Verifiable("Request send"); ;

            cartRepository.Setup(n => n.GetAllAsync()).ReturnsAsync(CartMockData.GetCartEntities().ToList());

            var handler = CartFactory.GetAllCartsHandler(mapper, cartRepository.Object);

            //act

            var mediatorResult = await CartFactory.GetAllCartDelegate.getAllCarts(mediatorMock.Object);
            var handlerResult = await handler.Handle(CartFactory.GetAllCartQuery, default);

            //assert
            mediatorMock.Verify(x => x.Send(It.IsAny<GetAllCartsQuery.Query>(),
                                          default),
                                          Times.Once());

            Assert.NotNull(mediatorResult);
            Assert.IsType<Ok<Response<GetAllCartsQuery.Result>>>(mediatorResult);

            var mediatorResponse = ((Ok<Response<GetAllCartsQuery.Result>>)mediatorResult).Value;

            Assert.IsType<Response<GetAllCartsQuery.Result>>(mediatorResponse);
            Assert.IsType<Response<GetAllCartsQuery.Result>>(handlerResult);

            Assert.True(mediatorResponse?.Payload?.GetAllCartsReponse.Carts.Count() == 3);
            Assert.True(handlerResult?.Payload?.GetAllCartsReponse.Carts.Count() == 3);

            ResetSetups();
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public async Task GetCartByIdTest(string requestId)
        {
            //arrange

            var entityResponse = CartMockData.GetCartEntities().FirstOrDefault(c => c.Id == requestId);

            var reponse = new Response<GetCartByIdQuery.Result>()
            {
                Payload = new GetCartByIdQuery.Result()
                {
                    GetCartByIdResponse = new GetCartByIdResponse()
                    {
                        Cart = mapper.Map<CartModel>(entityResponse),
                    }
                }
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<GetCartByIdQuery.Query>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(await Task.FromResult(reponse))
                .Verifiable("Request send"); ;

            cartRepository.Setup(n => n.GetByIdAsync(requestId)).ReturnsAsync(entityResponse);

            var handler = CartFactory.GetCartByIdHandler(mapper, cartRepository.Object);

            //act

            var mediatorResult = await CartFactory.GetCartByIdDelegate.GetCartByIdDelegate(mediatorMock.Object, requestId);
            var handlerResult = await handler.Handle(CartFactory.GetCartByIdQuery(requestId), default);

            //assert
            mediatorMock.Verify(x => x.Send(It.IsAny<Query>(),
                                          default),
                                          Times.Once());

            Assert.NotNull(mediatorResult);
            Assert.IsType<Ok<Response<Result>>>(mediatorResult);

            var mediatorResponse = ((Ok<Response<Result>>)mediatorResult).Value;

            Assert.IsType<Response<Result>>(mediatorResponse);
            Assert.IsType<Response<Result>>(handlerResult);

            Assert.NotNull(mediatorResponse?.Payload?.GetCartByIdResponse.Cart);
            Assert.NotNull(handlerResult?.Payload?.GetCartByIdResponse.Cart);

            Assert.IsType<CartModel>(mediatorResponse.Payload.GetCartByIdResponse.Cart);
            Assert.IsType<CartModel>(handlerResult.Payload.GetCartByIdResponse.Cart);

            Assert.True(mediatorResponse?.Payload?.GetCartByIdResponse.Cart.Id == mapper.Map<CartModel>(entityResponse).Id);
            Assert.True(handlerResult?.Payload?.GetCartByIdResponse.Cart.Id == mapper.Map<CartModel>(entityResponse).Id);

            ResetSetups();
        }

        [Theory]
        [InlineData("777")]
        [InlineData("888")]
        [InlineData("99")]
        public async Task GetCartByIdNotFoundTest(string requestId)
        {
            //arrange

            var statusCodeResponse = StatusCodes.Status422UnprocessableEntity;

            var entityResponse = CartMockData.GetCartEntities().FirstOrDefault(c => c.Id == requestId);

            var response = new BaseResponse
            {
                IsError = true,
                Errors = new List<ValidationMessage>(),
                StatusCode = statusCodeResponse
            };

            var json = JsonSerializer.Serialize(response);

            var data = System.Text.Encoding.UTF8.GetBytes(json);
            string actual = string.Empty;

            Mock<IHttpContextAccessor> httpContextMock = new Mock<IHttpContextAccessor>();

            httpContextMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object.HttpContext);

            httpContextMock.Setup(x => x.HttpContext.Response.ContentType).Returns("application/json");
            httpContextMock.Setup(x => x.HttpContext.Response.StatusCode).Returns(statusCodeResponse);

            httpContextMock.Setup(x => x.HttpContext.Response.Body.WriteAsync(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                        .Callback((byte[] data, int offset, int length, CancellationToken token) =>
                        {
                            if (length > 0)
                            {
                                actual = Encoding.UTF8.GetString(data);
                            }
                        }).Returns(Task.CompletedTask);

            Mock<IQueryHandler<Query, Response<Result>>> mockeHandler = new Mock<IQueryHandler<Query, Response<Result>>>();
            Mock<IExceptionMiddleware> exceptionMiddlewareMock = new Mock<IExceptionMiddleware>();

            exceptionMiddlewareMock.Setup(m => m.InvokeAsync(It.IsAny<HttpContext>())).Throws(new NotFoundException())
                .Verifiable("InvokeAsync");

            exceptionMiddlewareMock.Setup(m => m.HandleExceptionAsync(It.IsAny<HttpContext>(), It.IsAny<Exception>()))
                .Verifiable("F");

            mediatorMock.Setup(m => m.Send(It.IsAny<Query>(), It.IsAny<CancellationToken>()))
                .Throws(new NotFoundException())
                .Verifiable("Request send");

            cartRepository.Setup(n => n.GetByIdAsync(requestId)).ReturnsAsync(entityResponse);

            var handler = CartFactory.GetCartByIdHandler(mapper, cartRepository.Object);

            //act

            var delegateException = await Assert.ThrowsAsync<NotFoundException>(async () => await CartFactory.GetCartByIdDelegate.GetCartByIdDelegate(mediatorMock.Object, requestId));
            var handlerException = await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(CartFactory.GetCartByIdQuery(requestId), default));

            var exceptionHandlerExecute = await Assert.ThrowsAsync<NotFoundException>(async () => await exceptionMiddlewareMock.Object.InvokeAsync(httpContextMock.Object.HttpContext));
            await exceptionMiddlewareMock.Object.HandleExceptionAsync(httpContextMock.Object.HttpContext, handlerException);

            //assert

            mediatorMock.Verify(x => x.Send(It.IsAny<GetCartByIdQuery.Query>(),
                                         default),
                                         Times.Once());

            exceptionMiddlewareMock.Verify(x => x.InvokeAsync(It.IsAny<HttpContext>()), Times.Once(), "F");
            exceptionMiddlewareMock.Verify(x => x.HandleExceptionAsync(It.IsAny<HttpContext>(), handlerException), Times.Once());

            Assert.NotNull(handlerException);
            Assert.NotNull(delegateException);
            Assert.IsType<NotFoundException>(handlerException);
            Assert.IsType<NotFoundException>(delegateException);

            Assert.True(handlerException.Message.Equals("Cart do not exits"));

            Assert.IsType<NotFoundException>(exceptionHandlerExecute);

            ResetSetups();
        }

        [Fact]
        public async Task CreateCartOkTest()
        {
            //arrange

            var modelId = Random.Shared.Next().ToString();

            var command = CartFactory.CartCommand;
            var model = CartFactory.GetCartModel;
            var validationMock = CommonFactory.GetValidatorMock(CartFactory.CartCommand);
            var mapperMock = CommonFactory.GetMapperMock();

            command.Request.Products = CartMockData.GetCartModels().FirstOrDefault().Products;
            command.Request.ClientId = Random.Shared.Next().ToString();

            model = mapper.Map<CartModel>(command);
            model.Id = modelId;

            var response = new Response<CreateCartCommand.Result>();
            response.StatusCode = StatusCodes.Status200OK;
            response.Payload = new CreateCartCommand.Result();
            response.Payload.CreateCartResponse.Cart = model;

            mediatorMock.Setup(m => m.Send(It.IsAny<CreateCartCommand.Command>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable("Send");

            cartRepository.Setup(m => m.GetByIdAsync(model.Id)).ReturnsAsync((CartEntity)null);

            cartRepository.Setup(m => m.AddAsync(It.IsAny<CartEntity>()))
                .ReturnsAsync(mapper.Map<CartEntity>(model));

            mapperMock.Setup(x => x.Map<CreateCartCommand.Command, CartModel>(It.IsAny<CreateCartCommand.Command>()))
                .Returns(model);

            validationMock.Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>())).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            var handler = CartFactory.CreateCartCommandHandler(mapper, cartRepository.Object);

            //act

            var delegateResult = await CartFactory.CreateCartDelegate.CreateCart(mediatorMock.Object, command);
            var handlerResult = await handler.Handle(command, default);

            //assert

            mediatorMock.Verify(x => x.Send(It.IsAny<CreateCartCommand.Command>(), default), Times.Once);

            Assert.NotNull(handlerResult);
            Assert.NotNull(delegateResult);

            Assert.IsType<CreatedAtRoute>(delegateResult);
            Assert.IsType<Response<CreateCartCommand.Result>>(handlerResult);

            Assert.NotNull(handlerResult?.Payload?.CreateCartResponse.Cart);

            Assert.IsType<CartModel>(handlerResult.Payload.CreateCartResponse.Cart);

            Assert.False(string.IsNullOrEmpty(handlerResult?.Payload?.CreateCartResponse.Cart.Id));
            Assert.True(handlerResult?.Payload?.CreateCartResponse.Cart.Id.Equals(modelId));

            var cratedAtRoute = delegateResult as CreatedAtRoute;

            Assert.True(cratedAtRoute.StatusCode.Equals(StatusCodes.Status201Created));
            Assert.True(cratedAtRoute.RouteName.Equals("GetCartById"));
            Assert.True(cratedAtRoute.RouteValues.Values.FirstOrDefault().ToString() == modelId);

            ResetSetups();
        }

        private void ResetSetups()
        {
            mediatorMock.Reset();
            cartRepository.Reset();
        }
    }
}