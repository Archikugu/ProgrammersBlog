using AutoMapper;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Core.Utilities.Results.Concrete;
using ProgrammersBlog.DataAccess.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.CategoryDtos;

namespace ProgrammersBlog.Business.Concrete;

public class CategoryManager : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> Add(CategoryAddDto categoryAddDto, string createdByName)
    {
        //await _unitOfWork.Categories.AddAsync(new Category
        //{
        //    Name = categoryAddDto.Name,
        //    Description = categoryAddDto.Description,
        //    Note = categoryAddDto.Note,
        //    IsActive = categoryAddDto.IsActive,
        //    CreatedByName = createdByName,
        //    ModifiedByName = createdByName,
        //    CreatedDate = DateTime.Now,
        //    ModifiedDate = DateTime.Now,
        //    IsDeleted = false
        //}).ContinueWith(t => _unitOfWork.SaveAsync());
        //return new Result(ResultStatus.Success, $"{categoryAddDto.Name} category has been successfully added");

        //continue with -> await _unitOfWork.SaveAsync();

        // Add Auto Mapper

        var category = _mapper.Map<Category>(categoryAddDto);
        category.CreatedByName = createdByName;
        category.ModifiedByName = createdByName;
        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveAsync();
        return new Result(ResultStatus.Success, $"{categoryAddDto.Name} category has been successfully added");

    }
    public async Task<IResult> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
    {
        //var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);

        //if (category != null)
        //{
        //    category.Name = categoryUpdateDto.Name;
        //    category.Description = categoryUpdateDto.Description;
        //    category.Note = categoryUpdateDto.Note;
        //    category.IsActive = categoryUpdateDto.IsActive;
        //    category.IsDeleted = categoryUpdateDto.IsDeleted;
        //    category.ModifiedByName = modifiedByName;
        //    category.ModifiedDate = DateTime.Now;
        //    await _unitOfWork.Categories.UpdateAsync(category).ContinueWith(t => _unitOfWork.SaveAsync());
        //    return new Result(ResultStatus.Success, $"{categoryUpdateDto.Name} category has been successfully updated");
        //}
        //return new Result(ResultStatus.Error, "An error occurred while updating the category. Category cannot be found", null);

        //Add Auto Mapper

        var category = _mapper.Map<Category>(categoryUpdateDto);
        category.ModifiedByName = modifiedByName;
        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveAsync();
        return new Result(ResultStatus.Success, $"{categoryUpdateDto.Name} category has been successfully updated");
    }

    public async Task<IResult> Delete(int categoryId, string modifiedByName)
    {
        var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
        if (category != null)
        {
            category.IsDeleted = true;
            category.IsActive = false;
            category.ModifiedByName = modifiedByName;
            category.ModifiedDate = DateTime.Now;
            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, $"{category.Name} category has been successfully deleted");
        }
        return new Result(ResultStatus.Error, "An error occurred while deleting the category. Category cannot be found", null);
    }

    public async Task<IDataResult<CategoryDto>> Get(int categoryId)
    {
        var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
        if (category != null)
        {
            //return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
            //{
            //    Category = category,
            //    ResultStatus = ResultStatus.Success,
            //});
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return new DataResult<CategoryDto>(ResultStatus.Success, categoryDto);
        }
        return new DataResult<CategoryDto>(ResultStatus.Error, "An error occurred while fetching the category", null);

    }

    public async Task<IDataResult<CategoryListDto>> GetAll()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(null, c => c.Articles);
        if (categories.Count > -1)
        {
            //return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
            //{
            //    Categories = categories,
            //    ResultStatus = ResultStatus.Success
            //});

            var categoryDto = _mapper.Map<CategoryListDto>(categories);
            return new DataResult<CategoryListDto>(ResultStatus.Success, categoryDto);
        }
        return new DataResult<CategoryListDto>(ResultStatus.Error, "An error occurred while fetching the categories", null);
    }

    public async Task<IDataResult<CategoryListDto>> GetAllByNonDeleted()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted, c => c.Articles);
        if (categories.Count > -1)
        {
            //return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
            //{
            //    Categories = categories,
            //    ResultStatus = ResultStatus.Success
            //});
            var categoryDto = _mapper.Map<CategoryListDto>(categories);
            return new DataResult<CategoryListDto>(ResultStatus.Success, categoryDto);
        }
        return new DataResult<CategoryListDto>(ResultStatus.Error, "An error occurred while fetching the categories", null);
    }

    public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive, c => c.Articles);
        if (categories.Count > -1)
        {
            //return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
            //{
            //    Categories = categories,
            //    ResultStatus = ResultStatus.Success
            //});

            var categoryDto = _mapper.Map<CategoryListDto>(categories);
            return new DataResult<CategoryListDto>(ResultStatus.Success, categoryDto);
        }
        return new DataResult<CategoryListDto>(ResultStatus.Error, "An error occurred while fetching the categories", null);
    }

    public async Task<IResult> HardDelete(int categoryId)
    {
        var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
        if (category != null)
        {
            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, $"{category.Name} category has been successfully deleted");
        }
        return new Result(ResultStatus.Error, "An error occurred while deleting the category. Category cannot be found", null);
    }


}
