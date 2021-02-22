using System;
using System.Collections.Generic;
using System.Text;

namespace Lean.Email
{
    public class ViewRenderException : Exception
    {
        public ViewRenderException() { }

        public ViewRenderException(string message)
            : base(message) { }

        public ViewRenderException(string message, Exception inner)
            : base(message, inner) { }

        protected ViewRenderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
