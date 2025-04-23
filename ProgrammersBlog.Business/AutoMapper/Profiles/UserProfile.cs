using AutoMapper;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.UserDtos;

namespace ProgrammersBlog.Business.AutoMapper.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserAddDto, User>();
        CreateMap<User, UserUpdateDto>().ReverseMap();
    }
}
