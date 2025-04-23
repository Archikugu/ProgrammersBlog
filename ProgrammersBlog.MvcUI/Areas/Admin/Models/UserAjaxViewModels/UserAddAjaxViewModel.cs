using ProgrammersBlog.Entities.Dtos.UserDtos;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Models.UserAjaxViewModels;

public class UserAddAjaxViewModel
{
    public UserAddDto UserAddDto { get; set; }
    public string UserAddPartial { get; set; }
    public UserDto UserDto { get; set; }
}
