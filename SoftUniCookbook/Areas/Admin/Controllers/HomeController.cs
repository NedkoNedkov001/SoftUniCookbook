using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
