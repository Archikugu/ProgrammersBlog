using ProgrammersBlog.Core.Entities.Abstract;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.Entities.Dtos.UserDtos;

public class UserDto : DtoGetBase
{
    public User User { get; set; }
}
