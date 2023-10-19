using lab3.Commands;
using lab3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static lab3.Models.ShoppingCartPayedEvent;
using static lab3.Models.ShoppingCartState;
using static lab3.Operations.ShoppingCartStateOperations;

namespace lab3.Workflow
{
    internal class PayShoppingCartWorkflow
    {
        public IShoppingCartPayedEvent Execute(PayShoppingCartCommands command, Func<ProductName, bool> checkProductExists)
        {
            EmptyShoppingCart emptyShoppingCart = new EmptyShoppingCart(command.InputProducts);
            IShoppingCartState shoppingCartState = ValidatedShoppingCart(checkProductExists, emptyShoppingCart);
            shoppingCartState = CalculatePrice(shoppingCartState);
            shoppingCartState = PayShoppingCart(shoppingCartState);

            return shoppingCartState.Match(
                whenEmptyShoppingCart: emptyShoppingCart => new ShoppingCartPayFailedEvent("Unexpected result") as IShoppingCartPayedEvent,
                whenUnvalidatedShoppingCart: unvalidatedShoppingCart => new ShoppingCartPayFailedEvent(unvalidatedShoppingCart.Reason),
                whenValidatedShoppingCart: validatedShoppingCart => new ShoppingCartPayFailedEvent("Unexpected result"),
                whenCalculatedShoppingCart: calculatedShoppingCart => new ShoppingCartPayFailedEvent("Unexpected result"),
                whenPayedShoppingCart: payedShoppingCart => new ShoppingCartPaySuccededEvent(payedShoppingCart.Total)
            );
        }
    }
}
