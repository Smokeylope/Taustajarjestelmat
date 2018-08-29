using System;

namespace Citybike
{
    [Serializable()]
    public class NotFoundException : System.Exception
    {
        public NotFoundException() : base() {}
        public NotFoundException(string message) : base(message) {}
        public NotFoundException(string message, System.Exception inner) : base(message, inner) {}

        protected NotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) {}
    }
}