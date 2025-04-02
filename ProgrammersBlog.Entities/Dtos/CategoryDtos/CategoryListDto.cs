using ProgrammersBlog.Core.Entities.Abstract;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.Entities.Dtos.CategoryDtos;

public class CategoryListDto : DtoGetBase
{
    public IList<Category> Categories { get; set; }
}
