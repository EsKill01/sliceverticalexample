namespace TP.NA.CartServiceTest
{
    using CartService.Application.Models;
    using CartService.Domain.Entities;

    /// <summary>
    /// Cart mock data class
    /// </summary>
    public static class CartMockData
    {
        /// <summary>
        /// Get all carts using entity class
        /// </summary>
        /// <returns>A cart entity collection</returns>
        public static ICollection<CartEntity> GetCartEntities()
        {
            return new List<CartEntity>()
            {
                new CartEntity()
                {
                    Id = "1",
                    CreatedTime= DateTime.Now,
                    UserId= "1",
                    Products = new List<ProductEntity>{
                        new ProductEntity()
                        {
                            Id= "1",
                            Cost= 1,
                            Name = "AAAAA",
                            Quantity= 1,
                            Sku = "EEEEE"
                        },
                        new ProductEntity()
                        {
                            Id= "2",
                            Cost= 1.7M,
                            Name = "Soup",
                            Quantity = 3,
                            Sku = "RTRERT"
                        }
                    },
                },
                new CartEntity()
                {
                    Id = "2",
                    CreatedTime= DateTime.Now.AddDays(-3),
                    UserId= "2",
                    Products = new List<ProductEntity>{
                        new ProductEntity()
                        {
                            Id= "3",
                            Cost= 15,
                            Name = "POPOPOP",
                            Quantity= 1,
                            Sku = "PJPJKP"
                        },
                        new ProductEntity()
                        {
                            Id= "4",
                            Cost= 1.7M,
                            Name = "Soup",
                            Quantity = 3,
                            Sku = "TETESG"
                        }
                    },
                },
                new CartEntity()
                {
                    Id = "3",
                    CreatedTime= DateTime.Now.AddDays(-5),
                    UserId= "5",
                    Products = new List<ProductEntity>{
                        new ProductEntity()
                        {
                            Id= "2",
                            Cost= 9,
                            Name = "IREERF",
                            Quantity= 1,
                            Sku = "LERESF"
                        },
                        new ProductEntity()
                        {
                            Id= "8",
                            Cost= 1.1M,
                            Name = "Braed",
                            Quantity = 3,
                            Sku = "TETESGSSA"
                        }
                    },
                }
            };
        }

        /// <summary>
        /// Get all carts using model class
        /// </summary>
        /// <returns>A cart model collection</returns>
        public static ICollection<CartModel> GetCartModels()
        {
            return new List<CartModel>()
            {
                new CartModel()
                {
                    Id = "1",
                    CreatedTime= DateTime.Now,
                    UserId= "1",
                    Products = new List<ProductModel>{
                        new ProductModel()
                        {
                            Id= "1",
                            Cost= 1,
                            Name = "AAAAA",
                            Quantity= 1,
                            Sku = "EEEEE"
                        },
                        new ProductModel()
                        {
                            Id= "2",
                            Cost= 1.7M,
                            Name = "Soup",
                            Quantity = 3,
                            Sku = "RTRERT"
                        }
                    },
                },
                new CartModel()
                {
                    Id = "2",
                    CreatedTime= DateTime.Now.AddDays(-3),
                    UserId= "2",
                    Products = new List<ProductModel>{
                        new ProductModel()
                        {
                            Id= "3",
                            Cost= 15,
                            Name = "POPOPOP",
                            Quantity= 1,
                            Sku = "PJPJKP"
                        },
                        new ProductModel()
                        {
                            Id= "4",
                            Cost= 1.7M,
                            Name = "Soup",
                            Quantity = 3,
                            Sku = "TETESG"
                        }
                    },
                },
                new CartModel()
                {
                    Id = "3",
                    CreatedTime= DateTime.Now.AddDays(-5),
                    UserId= "5",
                    Products = new List<ProductModel>{
                        new ProductModel()
                        {
                            Id= "2",
                            Cost= 9,
                            Name = "IREERF",
                            Quantity= 1,
                            Sku = "LERESF"
                        },
                        new ProductModel()
                        {
                            Id= "8",
                            Cost= 1.1M,
                            Name = "Braed",
                            Quantity = 3,
                            Sku = "TETESGSSA"
                        }
                    },
                }
            };
        }
    }
}