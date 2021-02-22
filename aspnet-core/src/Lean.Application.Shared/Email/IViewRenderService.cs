using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Email
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName);
        Task<string> RenderToStringAsync<TModel>(string viewName, TModel model);
    }
}
