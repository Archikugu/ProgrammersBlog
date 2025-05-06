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

public class CategoryManager : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string createdByName)
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
        var addedCategory = await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveAsync();
        return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Add(addedCategory.Name),
            new CategoryDto
            {
                Category = addedCategory,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.Add(addedCategory.Name)
            });

    }
    public async Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
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

        var oldCategory = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);

        var category = _mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, oldCategory);
        category.ModifiedByName = modifiedByName;
        var updatedCategory = await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveAsync();
        return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.Update(updatedCategory.Name),
            new CategoryDto
            {
                Category = updatedCategory,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.Update(updatedCategory.Name)
            });
    }

    public async Task<IDataResult<CategoryDto>> Delete(int categoryId, string modifiedByName)
    {
        var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
        if (category != null)
        {
            category.IsDeleted = true;
            category.IsActive = false;
            category.ModifiedByName = modifiedByName;
            category.ModifiedDate = DateTime.Now;
            var deletedCategory = await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveAsync();
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

    public async Task<IDataResult<CategoryDto>> Get(int categoryId)
    {
        var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
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

    public async Task<IDataResult<CategoryListDto>> GetAll()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(null, c => c.Articles);
        if (categories.Count > -1)
        {
            return new DataResult<CategoryListDto>(ResultStatus.Error, new CategoryListDto
            {
                Categories = categories,
                ResultStatus = ResultStatus.Success,
            });
        }
        return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: true), new CategoryListDto
        {
            Categories = null,
            ResultStatus = ResultStatus.Error,
            Message = Messages.Category.NotFound(isPlural: true)
        });
    }

    public async Task<IDataResult<CategoryListDto>> GetAllByNonDeleted()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted, c => c.Articles);
        if (categories.Count > -1)
        {
            return new DataResult<CategoryListDto>(ResultStatus.Error, new CategoryListDto
            {
                Categories = categories,
                ResultStatus = ResultStatus.Success,
            });
        }
        return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: true), new CategoryListDto
        {
            Categories = null,
            ResultStatus = ResultStatus.Error,
            Message = Messages.Category.NotFound(isPlural: true)
        });
    }

    public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActive()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive, c => c.Articles);
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

    public async Task<IResult> HardDelete(int categoryId)
    {
        var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
        if (category != null)
        {
            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Category.HardDelete(category.Name));
        }
        return new Result(ResultStatus.Error, Messages.Category.HardDelete(category.Name), null);
    }

    public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDto(int categoryId)
    {
        var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
        if (result)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
            return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
        }
        else
        {
            return new DataResult<CategoryUpdateDto>(ResultStatus.Error, Messages.Category.NotFound(isPlural: false), null);
        }

    }

    public async Task<IDataResult<int>> Count()
    {
        var categoriesCount = await _unitOfWork.Categories.CountAsync();
        if (categoriesCount > -1)
        {
            return new DataResult<int>(ResultStatus.Success, categoriesCount);
        }
        else
        {
            return new DataResult<int>(ResultStatus.Error, "An unexpected error occurred", -1);
        }
    }

    public async Task<IDataResult<int>> CountByIsDeleted()
    {
        var categoriesCount = await _unitOfWork.Categories.CountAsync(c => !c.IsDeleted);
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
