using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.MvcUI.Models;

public class RightSideBarViewModel
{
    public IList<Category> Categories { get; set; }
    public IList<Article> Articles { get; set; }
}
