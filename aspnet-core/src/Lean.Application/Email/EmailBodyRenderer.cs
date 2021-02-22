using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Email
{
    public class EmailBodyRenderer : IEmailBodyRenderer
    {
        readonly IViewRenderService _viewRenderService;

        public EmailBodyRenderer(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        public async Task<string> Render<TModel>(string viewName, TModel model = null) where TModel : class
        {
            var htmlBody = await SafeRenderView(viewName, model);
            if (htmlBody == null)
            {
                throw new ViewNotFoundException($"Can't find any views for '{viewName}'. Searched views: '{viewName}'");
            }
            return htmlBody;
        }

        async Task<string> SafeRenderView<TModel>(string viewName, TModel model)
           where TModel : class
        {
            try
            {
                if (model == null)
                {
                    return await _viewRenderService.RenderToStringAsync(viewName);
                }

                return await _viewRenderService.RenderToStringAsync(viewName, model);
            }
            catch (ViewNotFoundException)
            {
                return null;
            }
        }
    }
}
