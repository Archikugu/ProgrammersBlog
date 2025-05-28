namespace ProgrammersBlog.Entities.Dtos.RoleDtos;

public class UserRoleAssignDto
{
    public UserRoleAssignDto()
    {
        RoleAssignDtos = new List<RoleAssignDto>();
    }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public IList<RoleAssignDto> RoleAssignDtos { get; set; }
}
