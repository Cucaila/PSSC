using lab3.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3.Models
{
    public record ProductPrice
    {
        public decimal Price { get; }

        public ProductPrice(decimal price)
        {
            if (price > 0)
            {
                Price = price;
            }
            else
            {
                throw new NegativePriceException("Price must be higher than 0");
            }
        }

        public static bool TryParseProductPrice(string priceString, out ProductPrice price)
        {
            bool isValid = false;
            price = null;

            if (decimal.TryParse(priceString, out decimal numericPrice))
            {
                if (numericPrice > 0)
                {
                    isValid = true;
                    price = new(numericPrice);
                }
            }

            return isValid;
        }
    }
}
