using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Layered
{
    [Serializable()]
    public class LevelTooLowException : System.Exception
    {
        public LevelTooLowException() : base() {}
        public LevelTooLowException(string message) : base(message) {}
        public LevelTooLowException(string message, System.Exception inner) : base(message, inner) {}

        protected LevelTooLowException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) {}
    }

    public class LevelTooLowExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is LevelTooLowException)
            {
                LevelTooLowException ex = context.Exception as LevelTooLowException;
                context.Result = new BadRequestResult();
            }
        }
    }
}