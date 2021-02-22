using System;
using System.Collections.Generic;
using System.Text;

namespace Lean.Email.Model
{
    public class ContactUsModel : CommonEmailModel
    {
        public string Message { get; set; }
        public string ReplyTo { get; set; }
    }
}
