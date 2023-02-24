using TP.NA.CartService.Application.Models;

namespace TP.NA.CartService.Application.Commands.Cart.Request
{
    public class CreateCartRequest
    {
        public CreateCartRequest()
        {
            Products = new List<ProductModel>();
        }

        public string ClientId { get; set; }

        public List<ProductModel> Products { get; set; }
    }
}