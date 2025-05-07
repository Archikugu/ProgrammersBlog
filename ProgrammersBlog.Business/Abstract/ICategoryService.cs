using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.CategoryDtos;

namespace ProgrammersBlog.Business.Abstract;

/// <summary>
/// Provides operations for managing categories such as adding, updating, deleting, and retrieving categories.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Retrieves a category with the specified ID.
    /// </summary>
    /// <param name="categoryId">The ID of the category to retrieve.</param>
    /// <returns>Returns a data result containing the category if found.</returns>
    Task<IDataResult<CategoryDto>> GetAsync(int categoryId);

    /// <summary>
    /// Returns the CategoryUpdateDto representation of the category with the given ID parameter.
    /// </summary>
    /// <param name="categoryId">An integer ID value greater than 0.</param>
    /// <returns>Returns the result as a DataResult of type Task through an asynchronous operation.</returns>
    Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId);

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <returns>Returns a list of all categories.</returns>
    Task<IDataResult<CategoryListDto>> GetAllAsync();

    /// <summary>
    /// Retrieves all categories that are not marked as deleted.
    /// </summary>
    /// <returns>Returns a list of non-deleted categories.</returns>
    Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync();

    /// <summary>
    /// Retrieves all categories that are both not deleted and active.
    /// </summary>
    /// <returns>Returns a list of active, non-deleted categories.</returns>
    Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync();

    /// <summary>
    /// Adds a new category asynchronously.
    /// </summary>
    /// <param name="categoryAddDto">The data transfer object containing the new category's information.</param>
    /// <param name="createdByName">The username of the person who created the category.</param>
    /// <returns>Returns a result wrapped in a Task that contains the added category and status information.</returns>
    Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName);

    /// <summary>
    /// Updates an existing category asynchronously.
    /// </summary>
    /// <param name="categoryUpdateDto">The DTO containing the updated category data.</param>
    /// <param name="modifiedByName">The username of the person who modified the category.</param>
    /// <returns>Returns a result wrapped in a Task that includes the updated category and operation status.</returns>
    Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName);

    /// <summary>
    /// Soft deletes a category by setting its IsDeleted and IsActive flags.
    /// </summary>
    /// <param name="categoryId">The ID of the category to be deleted.</param>
    /// <param name="modifiedByName">The username of the person performing the deletion.</param>
    /// <returns>Returns the result of the operation including the updated category data.</returns>
    Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName);

    /// <summary>
    /// Permanently deletes a category from the database.
    /// </summary>
    /// <param name="categoryId">The ID of the category to delete permanently.</param>
    /// <returns>Returns the result of the delete operation.</returns>
    Task<IResult> HardDeleteAsync(int categoryId);

    /// <summary>
    /// Counts the total number of categories in the database.
    /// </summary>
    /// <returns>Returns the total count as a data result.</returns>
    Task<IDataResult<int>> CountAsync();

    /// <summary>
    /// Counts the number of categories that are not deleted.
    /// </summary>
    /// <returns>Returns the count of non-deleted categories.</returns>
    Task<IDataResult<int>> CountByNonDeletedAsync();
}
