using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Models;

public class UserWithRolesViewModel
{
    public User User { get; set; }
    public IList<string> Roles { get; set; }
}
