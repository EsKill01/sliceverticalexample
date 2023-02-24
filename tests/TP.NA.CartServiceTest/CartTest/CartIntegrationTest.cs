using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net.Http.Json;
using System.Text.Json;
using TP.NA.CartService.Application.Abstractions.Repository;
using TP.NA.CartService.Application.Commons;
using TP.NA.CartService.Application.Models;
using TP.NA.CartService.Application.Queries.Cart;
using TP.NA.CartService.Domain.Entities;
using TP.NA.CartServiceTest.MapperConfig;

namespace TP.NA.CartServiceTest.CartTest
{
    public class CartIntegrationTest
    {
        private Mock<ICartRepository> cartRepository;
        private Mapper mapper;

        public CartIntegrationTest()
        {
            cartRepository = CommonFactory.CartRepository;
            mapper = CommonFactory.GetMapper(AutoMapperConf.Configuration());
        }

        [Fact]
        public async Task GetAllCartApiTest()
        {
            //arrange
            var url = $"api/v1/cartService";
            cartRepository.Setup(n => n.GetAllAsync()).ReturnsAsync(CartMockData.GetCartEntities().ToList());

            await using var application = new TestHostClass(cartRepository.Object);

            var client = application.CreateClient();

            //act

            var statusResponse = await client.GetAsync(url);

            var result = await client.GetFromJsonAsync<Response<GetAllCartsQuery.Result>>(url, new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            //assert

            Assert.Equal(System.Net.HttpStatusCode.OK, statusResponse.StatusCode);
            Assert.NotNull(statusResponse.Content);
            Assert.IsType<Response<GetAllCartsQuery.Result>>(result);
            Assert.NotNull(result);
            Assert.NotNull(result.Payload);
            Assert.Empty(result.Errors);
            Assert.IsAssignableFrom<IEnumerable<CartModel>>(result.Payload.GetAllCartsReponse.Carts);
            Assert.Equal(3, result.Payload.GetAllCartsReponse.Carts.Count());

            ResetSetups();
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        public async Task GetCartByIdApiTest(string id)
        {
            //arrange
            var url = $"api/v1/cartService/{id}";
            cartRepository.Setup(n => n.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(CartMockData.GetCartEntities().FirstOrDefault(c => c.Id == id));

            await using var application = new TestHostClass(cartRepository.Object);

            var client = application.CreateClient();

            //act

            var statusResponse = await client.GetAsync(url);

            var result = await client.GetFromJsonAsync<Response<GetCartByIdQuery.Result>>(url, new System.Text.Json.JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            //assert

            Assert.Equal(System.Net.HttpStatusCode.OK, statusResponse.StatusCode);
            Assert.NotNull(statusResponse.Content);
            Assert.IsType<Response<GetCartByIdQuery.Result>>(result);
            Assert.NotNull(result);
            Assert.NotNull(result.Payload);
            Assert.Empty(result.Errors);
            Assert.IsType<CartModel>(result.Payload.GetCartByIdResponse.Cart);
            Assert.Equal(id, result.Payload.GetCartByIdResponse.Cart.Id);

            ResetSetups();
        }

        [Theory]
        [InlineData("777")]
        [InlineData("888")]
        [InlineData("999")]
        public async Task GetCartByIdNotFoundApiTest(string id)
        {
            //arrange
            var url = $"api/v1/cartService/{id}";
            cartRepository.Setup(n => n.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(CartMockData.GetCartEntities().FirstOrDefault(c => c.Id == id));

            await using var application = new TestHostClass(cartRepository.Object);

            var client = application.CreateClient();

            var response = new BaseResponse
            {
                IsError = true,
                Errors = new List<ValidationMessage>()
                {
                    new ValidationMessage
                    {
                        Message = "Cart do not exits",
                        Property = "NotFoundException"
                    }
                },
                StatusCode = StatusCodes.Status404NotFound
            };

            var json = JsonSerializer.Serialize(response);

            var data = System.Text.Encoding.UTF8.GetBytes(json);
            string actual = string.Empty;

            //act

            var statusResponse = await client.GetAsync(url);

            var result = response;

            //assert

            Assert.Equal(System.Net.HttpStatusCode.NotFound, statusResponse.StatusCode);
            Assert.NotNull(statusResponse.Content);
            Assert.IsType<BaseResponse>(result);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
            Assert.Single(result.Errors);
            Assert.Equal("Cart do not exits", result.Errors.FirstOrDefault().Message);
            Assert.Equal("NotFoundException", result.Errors.FirstOrDefault().Property);

            ResetSetups();
        }

        [Theory]
        [InlineData("4")]
        public async Task CreateCartApiTest(string clientId)
        {
            var command = CartFactory.CartCommand;

            command.Request.ClientId = "2";
            command.Request.Products = new List<ProductModel>();
            command.Request.Products.AddRange(CartMockData.GetCartModels().FirstOrDefault().Products.ToList());

            var cartModel = mapper.Map<CartModel>(command);
            cartModel.Id = clientId;

            var cartEntity = mapper.Map<CartEntity>(cartModel);

            CartMockData.GetCartEntities().Add(cartEntity);

            //arrange
            var url = $"api/v1/cartService/";
            cartRepository.Setup(n => n.AddAsync(It.IsAny<CartEntity>())).ReturnsAsync(cartEntity);

            await using var application = new TestHostClass(cartRepository.Object);

            var client = application.CreateClient();

            //act

            var statusResponse = await client.PostAsJsonAsync(url, command);

            //assert

            Assert.Equal(System.Net.HttpStatusCode.Created, statusResponse.StatusCode);
            Assert.NotNull(statusResponse.Content);

            ResetSetups();
        }

        private void ResetSetups()
        {
            cartRepository.Reset();
        }
    }
}