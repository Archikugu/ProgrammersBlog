using Microsoft.AspNetCore.Mvc;

namespace ProgrammersBlog.MvcUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
