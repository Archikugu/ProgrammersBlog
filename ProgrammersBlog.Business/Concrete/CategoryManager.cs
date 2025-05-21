using AutoMapper;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Business.Utilities;
using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Core.Utilities.Results.Concrete;
using ProgrammersBlog.DataAccess.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.CategoryDtos;

namespace ProgrammersBlog.Business.Concrete;

public class CategoryManager : ManagerBase, ICategoryService
{

    public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }

    /// <summary>
    /// Adds a new category asynchronously.
    /// </summary>
    /// <param name="categoryAddDto">The data transfer object containing the new category's information.</param>
    /// <param name="createdByName">The username of the person who created the category.</param>
    /// <returns>Returns a result wrapped in a <see cref="Task"/> that contains the added category and status information.</returns>

    public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
    {
        var category = Mapper.Map<Category>(categoryAddDto);
        category.CreatedByName = createdByName;
        category.ModifiedByName = createdByName;
        var addedCategory = await UnitOfWork.Categories.AddAsync(category);
        await UnitOfWork.SaveAsync();
        return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Add(addedCategory.Name),
            new CategoryDto
            {
                Category = addedCategory,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.Add(addedCategory.Name)
            });
    }

    /// <summary>
    /// Updates an existing category asynchronously.
    /// </summary>
    /// <param name="categoryUpdateDto">The DTO containing the updated category data.</param>
    /// <param name="modifiedByName">The username of the person who modified the category.</param>
    /// <returns>Returns a result wrapped in a <see cref="Task"/> that includes the updated category and operation status.</returns>

    public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
    {
        var oldCategory = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);

        var category = Mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, oldCategory);
        category.ModifiedByName = modifiedByName;
        var updatedCategory = await UnitOfWork.Categories.UpdateAsync(category);
        await UnitOfWork.SaveAsync();
        return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Update(updatedCategory.Name),
            new CategoryDto
            {
                Category = updatedCategory,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.Update(updatedCategory.Name)
            });
    }

    /// <summary>
    /// Soft deletes a category by setting its IsDeleted and IsActive flags.
    /// </summary>
    /// <param name="categoryId">The ID of the category to be deleted.</param>
    /// <param name="modifiedByName">The username of the person performing the deletion.</param>
    /// <returns>Returns the result of the operation including the updated category data.</returns>
    public async Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName)
    {
        var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
        if (category != null)
        {
            category.IsDeleted = true;
            category.IsActive = false;
            category.ModifiedByName = modifiedByName;
            category.ModifiedDate = DateTime.Now;
            var deletedCategory = await UnitOfWork.Categories.UpdateAsync(category);
            await UnitOfWork.SaveAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Delete(deletedCategory.Name),
                new CategoryDto
                {
                    Category = deletedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.Delete(deletedCategory.Name)
                });
        }
        return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: false),
               new CategoryDto
               {
                   Category = null,
                   ResultStatus = ResultStatus.Error,
                   Message = Messages.Category.NotFound(isPlural: false)
               });
    }

    /// <summary>
    /// Retrieves a category with the specified ID.
    /// </summary>
    /// <param name="categoryId">The ID of the category to retrieve.</param>
    /// <returns>Returns a data result containing the category if found.</returns>
    public async Task<IDataResult<CategoryDto>> GetAsync(int categoryId)
    {
        var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
        if (category != null)
        {
            return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
            {
                Category = category,
                ResultStatus = ResultStatus.Success,
            });
        }
        return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: false), new CategoryDto
        {
            Category = null,
            ResultStatus = ResultStatus.Error,
            Message = Messages.Category.NotFound(isPlural: false)
        });
    }

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <returns>Returns a list of all categories.</returns>
    public async Task<IDataResult<CategoryListDto>> GetAllAsync()
    {
        var categories = await UnitOfWork.Categories.GetAllAsync(null);
        if (categories.Count > -1)
        {
            return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
            {
                Categories = categories,
                ResultStatus = ResultStatus.Success
            });
        }
        return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: true), new CategoryListDto
        {
            Categories = null,
            ResultStatus = ResultStatus.Error,
            Message = Messages.Category.NotFound(isPlural: true)
        });
    }

    /// <summary>
    /// Retrieves all categories that are not marked as deleted.
    /// </summary>
    /// <returns>Returns a list of non-deleted categories.</returns>
    public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync()
    {
        var categories = await UnitOfWork.Categories.GetAllAsync(c => !c.IsDeleted);
        if (categories.Count > -1)
        {
            return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
            {
                Categories = categories,
                ResultStatus = ResultStatus.Success
            });
        }
        return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: true), new CategoryListDto
        {
            Categories = null,
            ResultStatus = ResultStatus.Error,
            Message = Messages.Category.NotFound(isPlural: true)
        });
    }

    /// <summary>
    /// Retrieves all categories that are both not deleted and active.
    /// </summary>
    /// <returns>Returns a list of active, non-deleted categories.</returns>
    public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync()
    {
        var categories = await UnitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive);
        if (categories.Count > -1)
        {
            return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
            {
                Categories = categories,
                ResultStatus = ResultStatus.Success
            });
        }
        return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: true), null);
    }

    /// <summary>
    /// Permanently deletes a category from the database.
    /// </summary>
    /// <param name="categoryId">The ID of the category to delete permanently.</param>
    /// <returns>Returns the result of the delete operation.</returns>
    public async Task<IResult> HardDeleteAsync(int categoryId)
    {
        var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
        if (category != null)
        {
            await UnitOfWork.Categories.DeleteAsync(category);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Category.HardDelete(category.Name));
        }
        return new Result(ResultStatus.Error, Messages.Category.HardDelete(category.Name), null);
    }

    /// <summary>
    /// Returns the CategoryUpdateDto representation of the category with the given ID parameter.
    /// </summary>
    /// <param name="categoryId">An integer ID value greater than 0</param>
    /// <returns>Returns the result as a DataResult of type Task through an asynchronous operation</returns>

    public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId)
    {
        var result = await UnitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
        if (result)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            var categoryUpdateDto = Mapper.Map<CategoryUpdateDto>(category);
            return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
        }
        else
        {
            return new DataResult<CategoryUpdateDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: false), null);
        }

    }

    /// <summary>
    /// Counts the total number of categories in the database.
    /// </summary>
    /// <returns>Returns the total count as a data result.</returns>
    public async Task<IDataResult<int>> CountAsync()
    {
        var categoriesCount = await UnitOfWork.Categories.CountAsync();
        if (categoriesCount > -1)
        {
            return new DataResult<int>(ResultStatus.Success, categoriesCount);
        }
        else
        {
            return new DataResult<int>(ResultStatus.Error, "An unexpected error occurred", -1);
        }
    }


    /// <summary>
    /// Counts the number of categories that are not deleted.
    /// </summary>
    /// <returns>Returns the count of non-deleted categories.</returns>
    public async Task<IDataResult<int>> CountByNonDeletedAsync()
    {
        var categoriesCount = await UnitOfWork.Categories.CountAsync(c => !c.IsDeleted);
        if (categoriesCount > -1)
        {
            return new DataResult<int>(ResultStatus.Success, categoriesCount);
        }
        else
        {
            return new DataResult<int>(ResultStatus.Error, "An unexpected error occurred", -1);
        }
    }
}
