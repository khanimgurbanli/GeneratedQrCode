using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException() { }
        public AlreadyExistException(string message) : base(message) { }
    }
}
