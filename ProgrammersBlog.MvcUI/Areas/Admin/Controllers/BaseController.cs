using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.MvcUI.Helpers.Abstract;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Controllers;

public class BaseController : Controller
{

    public BaseController(UserManager<User> userManager, IMapper mapper, IImageHelper ımageHelper)
    {
        UserManager = userManager;
        Mapper = mapper;
        ImageHelper = ımageHelper;
    }
    protected UserManager<User> UserManager { get; }
    protected IMapper Mapper { get; }
    protected IImageHelper ImageHelper { get; }
    protected User LoggedInUser => UserManager.GetUserAsync(HttpContext.User).Result;
}
