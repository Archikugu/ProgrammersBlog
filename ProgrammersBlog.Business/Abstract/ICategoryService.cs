using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.CategoryDtos;

namespace ProgrammersBlog.Business.Abstract;

public interface ICategoryService
{
    Task<IDataResult<CategoryDto>> Get(int categoryId);
    Task<IDataResult<CategoryListDto>> GetAll();
    Task<IDataResult<CategoryListDto>> GetAllByNonDeleted();
    Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive();
    Task<IResult> Add(CategoryAddDto categoryAddDto, string createdByName);
    Task<IResult> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName);
    Task<IResult> Delete(int categoryId, string modifiedByName);
    Task<IResult> HardDelete(int categoryId);

}
