using ProgrammersBlog.Entities.Dtos.CategoryDtos;

namespace ProgrammersBlog.MvcUI.Areas.Admin.Models.CategoryAjaxViewModels;

public class CategoryUpdateAjaxViewModel
{
    public CategoryUpdateDto CategoryUpdateDto { get; set; }
    public string CategoryUpdatePartial { get; set; }
    public CategoryDto CategoryDto { get; set; }
}
