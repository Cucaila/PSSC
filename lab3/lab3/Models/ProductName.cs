using lab3.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3.Models
{
    public record ProductName
    {
        public string Name { get; }

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

        public static bool TryParseProductName(string nameString, out ProductName name)
        {
            bool isValid = false;
            name = null;

            if (!string.IsNullOrEmpty(nameString))
            {
                isValid = true;
                name = new ProductName(nameString);
            }

            return isValid;
        }
    }
}
