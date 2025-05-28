using ProgrammersBlog.Entities.Dtos.RoleDtos;
using ProgrammersBlog.Entities.Dtos.UserDtos;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Models.UserAjaxViewModels;

public class UserRoleAssignAjaxViewModel
{
    public UserRoleAssignDto UserRoleAssignDto { get; set; }
    public string RoleAssignPartial { get; set; }
    public UserDto UserDto { get; set; }
}
