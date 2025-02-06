using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS.Core.Exception
{
    public class ConcurencyException : System.Exception
    {
        public ConcurencyException()
        {
            
        }
    }
}