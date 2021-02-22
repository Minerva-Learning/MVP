using System;
using System.Collections.Generic;
using System.Text;

namespace Lean.Email
{
    public class ViewNotFoundException : Exception
    {
        public ViewNotFoundException() { }

        public ViewNotFoundException(string message)
            : base(message) { }

        public ViewNotFoundException(string message, Exception inner)
            : base(message, inner) { }

        protected ViewNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
