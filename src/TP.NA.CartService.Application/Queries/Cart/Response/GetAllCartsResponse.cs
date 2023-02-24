namespace TP.NA.CartService.Application.Queries.Cart.Response
{
    using TP.NA.CartService.Application.Commons;
    using TP.NA.CartService.Application.Models;

    /// <summary>
    /// Get all carts query
    /// </summary>
    public class GetAllCartsResponse : BaseResponse
    {
        /// <summary>
        /// List of carts
        /// </summary>
        public IEnumerable<CartModel> Carts { get; set; }
    }
}