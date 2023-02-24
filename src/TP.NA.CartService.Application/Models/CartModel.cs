namespace TP.NA.CartService.Application.Models
{
    /// <summary>
    /// Cart model
    /// </summary>
    public class CartModel
    {
        /// <summary>
        /// Gets or sets cart id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets user id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets if cart is active
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Gets or sets created time
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets update time
        /// </summary>
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        /// Gets or sets products list
        /// </summary>
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}