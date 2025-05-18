using AutoMapper;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Business.Utilities;
using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Core.Utilities.Results.Concrete;
using ProgrammersBlog.DataAccess.Abstract;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;
using Article = ProgrammersBlog.Entities.Concrete.Article;

namespace ProgrammersBlog.Business.Concrete;

public class ArticleManager : IArticleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName, int userId)
    {
        var article = _mapper.Map<Article>(articleAddDto);
        article.CreatedByName = createdByName;
        article.ModifiedByName = createdByName;
        article.UserId = userId;
        await _unitOfWork.Articles.AddAsync(article);
        await _unitOfWork.SaveAsync();
        return new Result(ResultStatus.Success, Messages.Article.Add(article.Title));
    }

    public async Task<IResult> DeleteAsync(int articleId, string modifiedByName)
    {
        var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
        if (result)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
            article.IsDeleted = true;
            article.IsActive = false;
            article.ModifiedByName = modifiedByName;
            article.ModifiedDate = DateTime.Now;
            await _unitOfWork.Articles.UpdateAsync(article);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Article.Delete(article.Title));
        }
        return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), null);
    }

    public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
    {
        var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.User, a => a.Category);
        if (article != null)
        {
            return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto
            {
                Article = article,
                ResultStatus = ResultStatus.Success
            });
        }
        return new DataResult<ArticleDto>(ResultStatus.Error, message: Messages.Article.NotFound(isPlural: false), data: null);
    }

    public async Task<IDataResult<ArticleListDto>> GetAllAsync()
    {
        var articles = await _unitOfWork.Articles.GetAllAsync(null, a => a.User, a => a.Category);
        if (articles.Count > -1)
        {
            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = articles,
                ResultStatus = ResultStatus.Success
            });
        }
        return new DataResult<ArticleListDto>(ResultStatus.Error, message: Messages.Article.NotFound(isPlural: true), data: null);
    }

    public async Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId)
    {
        var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
        if (result)
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, message: Messages.Article.NotFound(isPlural: true), data: null);
        }
        return new DataResult<ArticleListDto>(ResultStatus.Error, message: Messages.Category.NotFound(isPlural: true), data: null);
    }

    public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync()
    {
        var articles = await _unitOfWork.Articles.GetAllAsync(a => !a.IsDeleted, a => a.User, a => a.Category);
        if (articles.Count > -1)
        {
            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = articles,
                ResultStatus = ResultStatus.Success
            });
        }
        return new DataResult<ArticleListDto>(ResultStatus.Error, message: Messages.Article.NotFound(isPlural: true), data: null);
    }

    public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync()
    {
        var articles = await _unitOfWork.Articles.GetAllAsync(a => !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
        if (articles.Count > -1)
        {
            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = articles,
                ResultStatus = ResultStatus.Success
            });
        }
        return new DataResult<ArticleListDto>(ResultStatus.Error, message: Messages.Article.NotFound(isPlural: true), data: null);
    }

    public async Task<IResult> HardDeleteAsync(int articleId)
    {
        var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
        if (result)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
            await _unitOfWork.Articles.DeleteAsync(article);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Article.HardDelete(article.Title));
        }
        return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), null);
    }

    public async Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
    {
        var oldArticle = await _unitOfWork.Articles.GetAsync(a => a.Id == articleUpdateDto.Id);
        var article = _mapper.Map<ArticleUpdateDto, Article>(articleUpdateDto, oldArticle);
        article.ModifiedByName = modifiedByName; 

        await _unitOfWork.Articles.UpdateAsync(article);
        await _unitOfWork.SaveAsync();
        return new Result(ResultStatus.Success, Messages.Article.Update(article.Title));
    }

    public async Task<IDataResult<int>> CountAsync()
    {
        var articlesCount = await _unitOfWork.Articles.CountAsync();
        if (articlesCount > -1)
        {
            return new DataResult<int>(ResultStatus.Success, articlesCount);
        }
        else
        {
            return new DataResult<int>(ResultStatus.Error, "An unexpected error occurred", -1);
        }
    }

    public async Task<IDataResult<int>> CountByNonDeletedAsync()
    {
        var articlesCount = await _unitOfWork.Articles.CountAsync(a => !a.IsDeleted);
        if (articlesCount > -1)
        {
            return new DataResult<int>(ResultStatus.Success, articlesCount);
        }
        else
        {
            return new DataResult<int>(ResultStatus.Error, "An unexpected error occurred", -1);
        }
    }

    public async Task<IDataResult<ArticleUpdateDto>> GetArticleUpdateAsync(int articleId)
    {
        var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
        if (result)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
            var articleUpdateDto = _mapper.Map<ArticleUpdateDto>(article);
            return new DataResult<ArticleUpdateDto>(ResultStatus.Success, articleUpdateDto);
        }
        else
        {
            return new DataResult<ArticleUpdateDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), null);
        }
    }
}
