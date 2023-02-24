using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TP.NA.CartService.Application.Abstractions.Repository;

namespace TP.NA.CartServiceTest
{
    public class TestHostClass : WebApplicationFactory<Program>
    {
        private readonly ICartRepository _cartRepository;

        public TestHostClass(ICartRepository cartRepository)
        {
            this._cartRepository = cartRepository;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped(c => _cartRepository);
            });

            return base.CreateHost(builder);
        }
    }
}