using Microsoft.AspNetCore.Mvc;

namespace ProgrammersBlog.MvcUI.Controllers;

public class ErrorPageController : Controller
{
    [Route("ErrorPage/Error404")]
    [Route("ErrorPage/Error404/{code?}")]
    public IActionResult Error404(int? code = null)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AccessDenied()
    {
        return View();
    }
}
