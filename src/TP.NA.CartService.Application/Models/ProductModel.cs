namespace TP.NA.CartService.Application.Models
{
    /// <summary>
    /// Product model
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// Gets or sets product id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets product Sku
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Gets or sets product cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets cart quantity of this object
        /// </summary>
        public int Quantity { get; set; }
    }
}