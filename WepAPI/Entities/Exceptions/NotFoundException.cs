using System;

namespace Entities.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(String e) : base(e) { }
    }
}
