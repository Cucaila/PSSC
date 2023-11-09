using lab3.Models;
using LanguageExt;
using static LanguageExt.Prelude;
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

        public static Task<IShoppingCartState> ValidatedShoppingCart(Func<ProductName, TryAsync<bool>> checkProductExists, EmptyShoppingCart shoppingCart) =>
            shoppingCart.Products
                                .Select(ValidatedShoppingCartProduct(checkProductExists))
                                .Aggregate(CreateEmptyValatedProductList().ToAsync(), ReduceValidProduct)
                                .MatchAsync(
                                    Right: validatedProduct => new ValidateShoppingCart(validatedProduct),
                                    LeftAsync: errorMessage => Task.FromResult((IShoppingCartState)new InvalidShoppingCart(shoppingCart.Products, errorMessage)));

        private static Func<UnvalidatedShoppingCartProduct, EitherAsync<string, ValidatedShoppingCartProduct>> ValidatedShoppingCartProduct(Func<ProductName, TryAsync<bool>> checkProductExists) =>
            unvalidateCartProduct => ValidatedShoppingCart(checkProductExists, unvalidateCartProduct);

        private static EitherAsync<string, ValidatedShoppingCartProduct> ValidatedShoppingCart(Func<ProductName, TryAsync<bool>> checkProductExists, UnvalidatedShoppingCartProduct unvalidateCartProduct)
        {
            throw new NotImplementedException();
        }

        private static EitherAsync<string, ValidatedShoppingCartProduct> ValidateStudentGrade(Func<ProductName, TryAsync<bool>> checkProductExists, UnvalidatedShoppingCartProduct unvalidatedProduct) =>
           from examNameProduct in ProductName.TryParseProductName(unvalidatedProduct.productName)
                                  .ToEitherAsync(() => $"Invalid name product ({unvalidatedProduct.productName}, {unvalidatedProduct.productName})")
           from priceProduct in ProductPrice.TryParseProductPrice(unvalidatedProduct.price)
                                  .ToEitherAsync(() => $"Invalid price product ({unvalidatedProduct.price}, {unvalidatedProduct.price})")
           from productExists in checkProductExists(examNameProduct)
                                  .ToEither(error => error.ToString())
           select new ValidatedShoppingCartProduct(examNameProduct, priceProduct);


        private static Either<string, List<ValidatedShoppingCartProduct>> CreateEmptyValatedProductList() =>
            Right(new List<ValidatedShoppingCartProduct>());

        private static EitherAsync<string, List<ValidatedShoppingCartProduct>> ReduceValidProduct(EitherAsync<string, List<ValidatedShoppingCartProduct>> acc, EitherAsync<string, ValidatedShoppingCartProduct> next) =>
           from list in acc
           from nextProduct in next
           select list.AppendValidProduct(nextProduct);

        private static List<ValidatedShoppingCartProduct> AppendValidProduct(this List<ValidatedShoppingCartProduct> list, ValidatedShoppingCartProduct validProduct)
        {
            list.Add(validProduct);
            return list;
        }
        //public static IShoppingCartState ValidatedShoppingCart(Func<ProductName, bool> checkProductExists, EmptyShoppingCart shoppingCart)
        //{
        //    List<ValidatedShoppingCartProduct> validatedProducts = new List<ValidatedShoppingCartProduct>();
        //    bool isValidList = true;
        //    string invalidReson = string.Empty;
        //    foreach (var product in shoppingCart.Products)
        //    {
        //        if (!ProductName.TryParseProductName(product.productName, out ProductName productName))
        //        {
        //            invalidReson = $"Invalid product name: {product.productName}";
        //            isValidList = false;
        //            break;
        //        }
        //        if (!ProductPrice.TryParseProductPrice(product.price, out ProductPrice productPrice))
        //        {
        //            invalidReson = $"Invalid product price: {product.price}";
        //            isValidList = false;
        //            break;
        //        }
        //        ValidatedShoppingCartProduct validatedShoppingCartProduct = new(productName, productPrice);
        //        validatedProducts.Add(validatedShoppingCartProduct);
        //    }

        //    if (isValidList)
        //    {
        //        return new ValidatedShoppingCart(validatedProducts);
        //    }
        //    else
        //    {
        //        return new UnvalidatedShoppingCart(shoppingCart.Products, invalidReson);
        //    }
        //}

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
