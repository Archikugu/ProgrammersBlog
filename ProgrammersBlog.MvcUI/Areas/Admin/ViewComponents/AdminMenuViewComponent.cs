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

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var roles = await _userManager.GetRolesAsync(user);
        if (user == null)
            return Content("User Not Found!");

        if (roles == null)
            return Content("Roles Not Found!");

        return View(new UserWithRolesViewModel
        {
            User = user,
            Roles = roles
        });
    }
}
