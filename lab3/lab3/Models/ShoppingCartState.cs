using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3.Models
{
    [AsChoice]
    public static partial class ShoppingCartState
    {
        public interface IShoppingCartState { }

        public record EmptyShoppingCart : IShoppingCartState
        {
            public EmptyShoppingCart(IReadOnlyCollection<UnvalidatedShoppingCartProduct> products)
            {
                Products = products;
            }

            public IReadOnlyCollection<UnvalidatedShoppingCartProduct> Products { get; }
        }

        public record UnvalidatedShoppingCart : IShoppingCartState
        {
            public UnvalidatedShoppingCart(IReadOnlyCollection<UnvalidatedShoppingCartProduct> shoppingCartList, string reason)
            {
                ShoppingCartList = shoppingCartList;
                Reason = reason;
            }

            public IReadOnlyCollection<UnvalidatedShoppingCartProduct> ShoppingCartList { get; }
            public string Reason { get; }
        }

        public record ValidatedShoppingCart : IShoppingCartState
        {
            public ValidatedShoppingCart(IReadOnlyCollection<ValidatedShoppingCartProduct> shoppingCartList)
            {
                ShoppingCartProducts = shoppingCartList;
            }

            public IReadOnlyCollection<ValidatedShoppingCartProduct> ShoppingCartProducts { get; }
        }

        public record CalculatedShoppingCart : IShoppingCartState
        {
            public CalculatedShoppingCart(IReadOnlyCollection<CalculatedShoppingCartProduct> shoppingCartList)
            {
                ShoppingCartProducts = shoppingCartList;
            }

            public IReadOnlyCollection<CalculatedShoppingCartProduct> ShoppingCartProducts { get; }
        }

        public record PayedShoppingCart : IShoppingCartState
        {
            public PayedShoppingCart(IReadOnlyCollection<CalculatedShoppingCartProduct> shoppingCartList, decimal total)
            {
                ShoppingCartList = shoppingCartList;
                Total = total;
            }

            public IReadOnlyCollection<CalculatedShoppingCartProduct> ShoppingCartList { get; }
            public decimal Total { get; }
        }
    }
}
