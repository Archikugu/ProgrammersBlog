using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.MvcUI.Areas.Admin.Models;

namespace ProgrammersBlog.MvcUI.Areas.Admin.ViewComponents;

public class AdminMenuViewComponent : ViewComponent
{
    private readonly UserManager<User> _userManager;

    public AdminMenuViewComponent(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public IViewComponentResult Invoke()
    {
        var user = _userManager.GetUserAsync(HttpContext.User).Result;
        var roles = _userManager.GetRolesAsync(user).Result;
        return View(new UserWithRolesViewModel
        {
            User = user,
            Roles = roles
        });
    }
}
