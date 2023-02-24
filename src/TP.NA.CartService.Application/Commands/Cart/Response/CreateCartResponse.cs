namespace TP.NA.CartService.Application.Commands.Cart.Response
{
    using TP.NA.CartService.Application.Commons;
    using TP.NA.CartService.Application.Models;

    /// <summary>
    /// Create cart response
    /// </summary>
    public class CreateCartResponse : BaseResponse
    {
        /// <summary>
        /// Gets or sets cart model
        /// </summary>
        public CartModel Cart { get; set; }
    }
}