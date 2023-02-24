namespace TP.NA.CartService.Repository.Cart
{
    using Microsoft.Extensions.Logging.Abstractions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TP.NA.CartService.Application.Abstractions.Repository;
    using TP.NA.CartService.Domain.Entities;
    using TP.NA.Common.Repository.Extensions;
    using TP.NA.Common.Repository.Persistance;

    /// <summary>
    /// Cart repository implementation
    /// </summary>
    public class CartRepository : ICartRepository
    {
        private ICosmosRepository<CartEntity> _cartRepository;

        public CartRepository(ICosmosRepository<CartEntity> cosmosRepository)
        {
            _cartRepository = cosmosRepository;
        }

        public async Task<CartEntity?> AddAsync(CartEntity entity)
        {

            var res = await _cartRepository.Upsert(entity);

            if (res.StatusCode != System.Net.HttpStatusCode.Created)
            {
                return null;
            }

            return await Task.FromResult(res.Model);
        }

        public async Task<CartEntity> AddProductAsync(string cartId, ProductEntity product)
        {
            var cart = (await (await _cartRepository.Get(cartId)).LinqQueryToResults()).FirstOrDefault();
            cart.Products.Add(product);
            var res = await _cartRepository.Upsert(cart);

            if (res.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }
            return await Task.FromResult(res.Model);
        }

        public async Task<CartEntity> DeleteProductAsync(string cartId, string productId)
        {
            var cart = (await (await _cartRepository.Get(cartId)).LinqQueryToResults()).FirstOrDefault();

            var product = cart.Products.FirstOrDefault(x => x.Id == productId);
            cart.Products.Remove(product);

            var res = await _cartRepository.Upsert(cart);

            if (res.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            return await Task.FromResult(res.Model);
        }

        public async Task<IEnumerable<CartEntity>> GetAllAsync()
        {
            var all = await (await _cartRepository.Get()).LinqQueryToResults();

            return await Task.FromResult(all);
        }

        public async Task<CartEntity?> GetByIdAsync(string id)
        {
            var found = (await (await _cartRepository.Get(id)).LinqQueryToResults()).FirstOrDefault();

            return await Task.FromResult(found);
        }

        public async Task<CartEntity> UpdateProductQuantityAsync(string cartId, string productId, int quantity)
        {
            var cart = (await (await _cartRepository.Get(cartId)).LinqQueryToResults()).FirstOrDefault();

            cart.Products.FirstOrDefault(c => c.Id == productId).Quantity = quantity;

            var res = await _cartRepository.Upsert(cart);

            if (res.StatusCode != System.Net.HttpStatusCode.Created)
            {
                return null;
            }

            return await Task.FromResult(cart);
        }
    }
}