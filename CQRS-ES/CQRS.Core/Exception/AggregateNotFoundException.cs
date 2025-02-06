using System;
namespace CQRS.Core.Exception
{
    public class AggregateNotFoundException : System.Exception
    {
        
        public AggregateNotFoundException(string message) : base(message)
        {
            
        }
    }
}