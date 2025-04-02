using ProgrammersBlog.Core.Entities.Abstract;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.Entities.Dtos.CategoryDtos;

public class CategoryDto : DtoGetBase
{
    public Category Category { get; set; }
}
