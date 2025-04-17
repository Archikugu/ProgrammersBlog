using Microsoft.AspNetCore.Identity;
using ProgrammersBlog.Core.Entities.Abstract;

namespace ProgrammersBlog.Entities.Concrete;

public class User : IdentityUser<int>
{
    public string Picture { get; set; }
    public ICollection<Article> Articles { get; set; }
}
