using TP.NA.CartService.Application.Models;

namespace TP.NA.CartService.Application.Commands.Product.Request
{
    public class AddProductCartRequest
    {
        public string ClientId { get; set; }

        public ProductModel Product { get; set; }

        public AddProductCartRequest()
        {
            Product = new ProductModel();
        }
    }
}