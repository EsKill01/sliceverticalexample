using System;
using System.Collections.Generic;
using TP.NA.CartService.Domain.Entities;

namespace TP.NA.CartService.Repository.Cart
{
    public static class CartMemoryFactory
    {
        public static List<CartEntity> listCart = new List<CartEntity>
       {
            new CartEntity
            {
                Id = "1",
                CreatedTime = DateTime.Now,
                UserId = "1",
                Products = new List<ProductEntity>
                {
                    new ProductEntity
                    {
                        Quantity= 1,
                        Cost= 1,
                        Name = "aaaaa",
                        Id = "1"
                    },
                    new ProductEntity
                    {
                        Quantity= 1,
                        Cost= 1,
                        Name = "eeeee",
                        Id = "2"
                    }
                }
            },
            new CartEntity
            {
                Id = "2",
                CreatedTime = DateTime.Now,
                UserId = "2",
                Products = new List<ProductEntity>
                {
                    new ProductEntity
                    {
                        Quantity= 1,
                        Cost= 1,
                        Id= "55",
                        Name = "SSSS"
                    },
                    new ProductEntity
                    {
                        Quantity= 3,
                        Name = "AAAA",
                        Id = "12",
                        Cost = 11
                    }
                }
            }
        };
    }
}