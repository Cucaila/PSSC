using lab3.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace lab3.Models
{
    public record ProductName
    {
        public string Name { get; }
        private static readonly Regex ValidPattern = new("^LM[0-9]{5}$");

        public ProductName(string name)
        {
            if (name.Length > 1)
            {
                Name = name;
            }
            else
            {
                throw new InvalidProductNameException("Length must be greater than 1");
            }
        }
        private static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Name;
        }
        public static Option<ProductName> TryParseProductName(string nameString)
        {
            if (IsValid(nameString))
            {
                return Some<ProductName>(new(nameString));
            }
            else
            {
                return None;
            }
        }
    }
}
