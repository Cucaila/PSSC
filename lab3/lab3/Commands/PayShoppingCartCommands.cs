using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lab3.Models;

namespace lab3.Commands
{
    public record PayShoppingCartCommands
    {
        public PayShoppingCartCommands(IReadOnlyCollection<UnvalidatedShoppingCartProduct> products)
        {
            InputProducts = products;
        }
        public IReadOnlyCollection<UnvalidatedShoppingCartProduct> InputProducts { get; }
    }
}
