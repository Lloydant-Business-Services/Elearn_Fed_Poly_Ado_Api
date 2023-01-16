using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }
}
