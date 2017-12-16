using System;

namespace Entities.Exceptions
{
    public class AlreadyThereException : Exception
    {
        public AlreadyThereException(String e) : base(e)
        {
          
        }
    }
}
