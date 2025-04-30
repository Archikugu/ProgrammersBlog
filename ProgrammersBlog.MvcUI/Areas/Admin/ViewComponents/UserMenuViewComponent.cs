using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.MvcUI.Areas.Admin.Models;

namespace ProgrammersBlog.MvcUI.Areas.Admin.ViewComponents;

public class UserMenuViewComponent : ViewComponent
{
    private readonly UserManager<User> _userManager;

    public UserMenuViewComponent(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public IViewComponentResult Invoke()
    {
        var user = _userManager.GetUserAsync(HttpContext.User).Result;
        return View(new UserViewModel
        {
            User = user
        });
    }
}
