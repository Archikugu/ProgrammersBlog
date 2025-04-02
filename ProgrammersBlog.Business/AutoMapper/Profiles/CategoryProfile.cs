using AutoMapper;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.CategoryDtos;

namespace ProgrammersBlog.Business.AutoMapper.Profiles;

public class CategoryProfile: Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryAddDto, Category>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));
        CreateMap<CategoryUpdateDto, Category>().ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));

        CreateMap<Category, CategoryDto>();
        CreateMap<List<Category>, CategoryListDto>();
    }
}
