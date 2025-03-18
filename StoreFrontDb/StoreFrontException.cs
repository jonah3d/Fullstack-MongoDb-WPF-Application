using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFrontDb
{
    public  class StoreFrontException : Exception
    {
        public StoreFrontException() { }
        public StoreFrontException(string message) : base(message) { }
        public StoreFrontException(string message, Exception inner) : base(message, inner) { }
    }
}
