using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Email
{
    /// <summary>
    /// Borrowed from https://long2know.com/2017/08/rendering-and-emailing-embedded-razor-views-with-net-core/
    /// </summary>
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISiteRootProvider _siteRootProvider;

        public ViewRenderService(
            IRazorViewEngine viewEngine,
            IHttpContextAccessor httpContextAccessor,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            ISiteRootProvider siteRootProvider)
        {
            _viewEngine = viewEngine;
            _httpContextAccessor = httpContextAccessor;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _siteRootProvider = siteRootProvider;
        }

        [Obsolete]
        public string RenderToString<TModel>(string viewPath, TModel model)
        {
            try
            {
                var viewEngineResult = _viewEngine.GetView("~/", viewPath, false);

                if (!viewEngineResult.Success)
                {
                    throw new InvalidOperationException($"Couldn't find view {viewPath}");
                }

                var view = viewEngineResult.View;

                using (var sw = new StringWriter())
                {
                    var viewContext = new ViewContext()
                    {
                        HttpContext = _httpContextAccessor.HttpContext ?? new DefaultHttpContext { RequestServices = _serviceProvider },
                        ViewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model },
                        Writer = sw
                    };
                    AddCommonEmailInfo(viewContext.ViewData);

                    view.RenderAsync(viewContext).GetAwaiter().GetResult();
                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new ViewRenderException($"Error rendering view '{viewPath}'.", ex);
            }
        }

        public async Task<string> RenderToStringAsync<TModel>(string viewName, TModel model)
        {
            var httpContext = _httpContextAccessor.HttpContext ?? new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(actionContext, viewName, false);

                // Fallback - the above seems to consistently return null when using the EmbeddedFileProvider
                if (viewResult.View == null)
                {
                    viewResult = _viewEngine.GetView("~/", viewName, false);
                }

                if (viewResult.View == null)
                {
                    throw new ViewNotFoundException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };
                AddCommonEmailInfo(viewDictionary);
                try
                {
                    var viewContext = new ViewContext(
                        actionContext,
                        viewResult.View,
                        viewDictionary,
                        new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                        sw,
                        new HtmlHelperOptions());
                    await viewResult.View.RenderAsync(viewContext);
                }
                catch (Exception ex)
                {
                    throw new ViewRenderException($"Error in rendering view '{viewName}'.", ex);
                }

                return sw.ToString();
            }
        }

        [Obsolete]
        public string RenderToString(string viewPath)
        {
            return RenderToString(viewPath, string.Empty);
        }

        public Task<string> RenderToStringAsync(string viewName)
        {
            return RenderToStringAsync<string>(viewName, string.Empty);
        }

        private void AddCommonEmailInfo(ViewDataDictionary viewDictionary)
        {
            if (_siteRootProvider != null)
            {
                viewDictionary["EmailLogo"] = $"{_siteRootProvider.GetSiteRoot()}/api/Settings/EmailLogo";
                viewDictionary["SiteRoot"] = $"{_siteRootProvider.GetSiteRoot()}";
            }
        }
    }
}
