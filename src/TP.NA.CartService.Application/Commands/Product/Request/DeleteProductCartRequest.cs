namespace TP.NA.CartService.Application.Commands.Product.Request
{
    public class DeleteProductCartRequest
    {
        public string ClientId { get; set; }

        public string ProductId { get; set; }
    }
}