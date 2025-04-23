using ProgrammersBlog.Entities.Dtos.CategoryDtos;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Models.CategoryAjaxViewModels;

public class CategoryAddAjaxViewModel
{
    public CategoryAddDto CategoryAddDto { get; set; }
    public string CategoryAddPartial { get; set; }
    public CategoryDto CategoryDto { get; set; }
}
