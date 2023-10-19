using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3.Exceptions
{
    internal class InvalidProductNameException : Exception
    {
        public InvalidProductNameException(string message) : base(message) { }
    }
}
