using lab3.Exceptions;
using LanguageExt;
using static LanguageExt.Prelude;



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

        public static Option<ProductPrice> TryParseProductPrice(string priceString)
        {
            if (decimal.TryParse(priceString, out decimal numericPrice) && isValid(numericPrice))
            {
                return Some<ProductPrice>(new(numericPrice));
            }
            else
            {
                return None;
            } 
        }
        private static bool isValid(decimal numericPrice) => numericPrice > 0;
    }
}
