using AutoMapper;
using Moq;
using System.Net.Http.Json;
using TP.NA.CartService.Application.Abstractions.Repository;
using TP.NA.CartService.Domain.Entities;
using TP.NA.CartServiceTest.MapperConfig;

namespace TP.NA.CartServiceTest.ProductTest
{
    public class ProductIntegrationTest
    {
        private Mock<ICartRepository> cartRepository;
        private Mapper mapper;

        public ProductIntegrationTest()
        {
            cartRepository = CommonFactory.CartRepository;
            mapper = CommonFactory.GetMapper(AutoMapperConf.Configuration());
        }

        [Theory]
        [InlineData("1", "99")]
        public async Task AddProductCartApiTest(string clientId, string productId)
        {
            var command = ProductFactory.AddProductCommand;

            command.Request.ClientId = clientId;
            command.Request.Product = CartMockData.GetCartModels().FirstOrDefault().Products.FirstOrDefault();
            command.Request.Product.Id = productId;

            var cartEntity = CartMockData.GetCartEntities().FirstOrDefault();

            //arrange
            var url = $"api/v1/cartService/addProduct";

            cartRepository.Setup(n => n.GetAllAsync()).ReturnsAsync(CartMockData.GetCartEntities());
            cartRepository.Setup(n => n.AddProductAsync(It.IsAny<string>(), It.IsAny<ProductEntity>())).ReturnsAsync(cartEntity);

            await using var application = new TestHostClass(cartRepository.Object);

            var client = application.CreateClient();

            //act

            var statusResponse = await client.PostAsJsonAsync(url, command);

            //assert

            Assert.Equal(System.Net.HttpStatusCode.OK, statusResponse.StatusCode);
            Assert.NotNull(statusResponse.Content);

            ResetSetups();
        }

        [Theory]
        [InlineData("1", "1")]
        [InlineData("99", "1")]
        public async Task AddProductCartBadRequestApiTest(string clientId, string productId)
        {
            var command = ProductFactory.AddProductCommand;

            command.Request.ClientId = clientId;
            command.Request.Product = CartMockData.GetCartModels().FirstOrDefault().Products.FirstOrDefault();
            command.Request.Product.Id = productId;

            var cartEntity = CartMockData.GetCartEntities().FirstOrDefault();

            //arrange
            var url = $"api/v1/cartService/addProduct";

            cartRepository.Setup(n => n.GetAllAsync()).ReturnsAsync(CartMockData.GetCartEntities());
            cartRepository.Setup(n => n.AddProductAsync(It.IsAny<string>(), It.IsAny<ProductEntity>())).ReturnsAsync(cartEntity);

            await using var application = new TestHostClass(cartRepository.Object);

            var client = application.CreateClient();

            //act

            var statusResponse = await client.PostAsJsonAsync(url, command);

            //assert

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, statusResponse.StatusCode);
            Assert.NotNull(statusResponse.Content);

            ResetSetups();
        }

        [Theory]
        [InlineData("1", "1")]
        public async Task DeleteProductCartApiTest(string clientId, string productId)
        {
            var command = ProductFactory.DeleteProductCommand;

            command.Request.ClientId = clientId;
            command.Request.ProductId = productId;

            var cartEntity = CartMockData.GetCartEntities().FirstOrDefault();

            //arrange
            var url = $"api/v1/cartService/removeProduct";

            cartRepository.Setup(n => n.GetAllAsync()).ReturnsAsync(CartMockData.GetCartEntities());
            cartRepository.Setup(n => n.DeleteProductAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(cartEntity);

            await using var application = new TestHostClass(cartRepository.Object);

            var client = application.CreateClient();

            //act

            var statusResponse = await client.PostAsJsonAsync(url, command);

            //assert

            Assert.Equal(System.Net.HttpStatusCode.OK, statusResponse.StatusCode);
            Assert.NotNull(statusResponse.Content);

            ResetSetups();
        }

        [Theory]
        [InlineData("11", "1")]
        [InlineData("1", "99")]
        public async Task DeleteProductCartBadRequestApiTest(string clientId, string productId)
        {
            var command = ProductFactory.DeleteProductCommand;

            command.Request.ClientId = clientId;
            command.Request.ProductId = productId;

            var cartEntity = CartMockData.GetCartEntities().FirstOrDefault();

            //arrange
            var url = $"api/v1/cartService/removeProduct";

            cartRepository.Setup(n => n.GetAllAsync()).ReturnsAsync(CartMockData.GetCartEntities());
            cartRepository.Setup(n => n.DeleteProductAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(cartEntity);

            await using var application = new TestHostClass(cartRepository.Object);

            var client = application.CreateClient();

            //act

            var statusResponse = await client.PostAsJsonAsync(url, command);

            //assert

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, statusResponse.StatusCode);
            Assert.NotNull(statusResponse.Content);

            ResetSetups();
        }

        private void ResetSetups()
        {
            cartRepository.Reset();
        }
    }
}