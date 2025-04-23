using ProgrammersBlog.Core.Entities.Abstract;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.Entities.Dtos.UserDtos;

public class UserListDto : DtoGetBase
{
    public IList<User> Users { get; set; }
}
