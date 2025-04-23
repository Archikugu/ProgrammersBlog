using ProgrammersBlog.Entities.Dtos.UserDtos;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Models.UserAjaxViewModels;

public class UserUpdateAjaxViewModel
{
    public UserUpdateDto UserUpdateDto { get; set; }
    public string UserUpdatePartial { get; set; }
    public UserDto UserDto { get; set; }
}
