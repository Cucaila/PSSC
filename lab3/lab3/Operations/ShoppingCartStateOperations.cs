using lab3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static lab3.Models.ShoppingCartState;

namespace lab3.Operations
{
    public static class ShoppingCartStateOperations
    {
        public static IShoppingCartState ValidatedShoppingCart(Func<ProductName, bool> checkProductExists, EmptyShoppingCart shoppingCart)
        {
            List<ValidatedShoppingCartProduct> validatedProducts = new List<ValidatedShoppingCartProduct>();
            bool isValidList = true;
            string invalidReson = string.Empty;
            foreach (var product in shoppingCart.Products)
            {
                if (!ProductName.TryParseProductName(product.productName, out ProductName productName))
                {
                    invalidReson = $"Invalid product name: {product.productName}";
                    isValidList = false;
                    break;
                }
                if (!ProductPrice.TryParseProductPrice(product.price, out ProductPrice productPrice))
                {
                    invalidReson = $"Invalid product price: {product.price}";
                    isValidList = false;
                    break;
                }
                ValidatedShoppingCartProduct validatedShoppingCartProduct = new(productName, productPrice);
                validatedProducts.Add(validatedShoppingCartProduct);
            }

            if (isValidList)
            {
                return new ValidatedShoppingCart(validatedProducts);
            }
            else
            {
                return new UnvalidatedShoppingCart(shoppingCart.Products, invalidReson);
            }
        }

        public static IShoppingCartState CalculatePrice(IShoppingCartState shoppingCart) =>
            shoppingCart.Match(
                whenEmptyShoppingCart: emptyShoppingCart => emptyShoppingCart,
                whenUnvalidatedShoppingCart: unvalidatedShoppingCart => unvalidatedShoppingCart,
                whenCalculatedShoppingCart: calculatedShoppingCart => calculatedShoppingCart,
                whenPayedShoppingCart: payedShoppingCart => payedShoppingCart,
                whenValidatedShoppingCart: validatedShoppingCart =>
                {
                    var calculatedPrice = validatedShoppingCart.ShoppingCartProducts.Select(validProduct =>
                        new CalculatedShoppingCartProduct(validProduct.productName, validProduct.price)
                    );
                    return new CalculatedShoppingCart(calculatedPrice.ToList().AsReadOnly());
                }
                );

        public static IShoppingCartState PayShoppingCart(IShoppingCartState shoppingCart) =>
            shoppingCart.Match(
                whenEmptyShoppingCart: emptyShoppingCart => emptyShoppingCart,
                whenUnvalidatedShoppingCart: unvalidatedShoppingCart => unvalidatedShoppingCart,
                whenPayedShoppingCart: payedShoppingCart => payedShoppingCart,
                whenValidatedShoppingCart: validatedShoppingCart => validatedShoppingCart,
                whenCalculatedShoppingCart: calculatedShoppingCart =>
                {
                    decimal total = 0;
                    foreach (var product in calculatedShoppingCart.ShoppingCartProducts)
                    {
                        total += product.price.Price;
                    }

                    PayedShoppingCart payedShoppingCart = new(calculatedShoppingCart.ShoppingCartProducts, total);
                    return payedShoppingCart;
                }
                );
    }
}
