using System;
using System.Collections.Generic;
using System.Text;

namespace Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(String E) : base(E) { }
    }
}
