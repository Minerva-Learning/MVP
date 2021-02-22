using Microsoft.AspNetCore.Mvc;
using Lean.Web.Controllers;

namespace Lean.Web.Public.Controllers
{
    public class AboutController : LeanControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}