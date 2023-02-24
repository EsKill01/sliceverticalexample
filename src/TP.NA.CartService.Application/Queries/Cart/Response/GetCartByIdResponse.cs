namespace TP.NA.CartService.Application.Queries.Cart.Response
{
    using TP.NA.CartService.Application.Models;

    /// <summary>
    /// Get cart by id
    /// </summary>
    public class GetCartByIdResponse
    {
        /// <summary>
        /// Gets or sets cart
        /// </summary>
        public CartModel Cart { get; set; }
    }
}