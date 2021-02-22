using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Email
{
    public interface IEmailBodyRenderer
    {
        Task<string> Render<TModel>(string emailId, TModel model = null)
            where TModel : class;
    }
}
