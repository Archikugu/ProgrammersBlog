using AutoMapper;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Core.Utilities.Results.Concrete;
using ProgrammersBlog.DataAccess.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;
using ProgrammersBlog.Entities.Dtos.CategoryDtos;

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

    public async Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName)
    {
        var article = _mapper.Map<Article>(articleAddDto);
        article.CreatedByName = createdByName;
        article.ModifiedByName = createdByName;
        article.UserId = 1;
        await _unitOfWork.Articles.AddAsync(article);
        await _unitOfWork.SaveAsync();
        return new Result(ResultStatus.Success, $"{articleAddDto.Title} article has been successfully added");
    }

    public async Task<IResult> Delete(int articleId, string modifiedByName)
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
            return new Result(ResultStatus.Success, $"{article.Title} article has been successfully deleted");
        }
        return new Result(ResultStatus.Error, "An error occurred while deleting the article. Article cannot be found", null);
    }

    public async Task<IDataResult<ArticleDto>> Get(int articleId)
    {
        var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.User, a => a.Category);
        if (article != null)
        {
            //return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto
            //{
            //    Article = article,
            //    ResultStatus = ResultStatus.Success
            //});

            var articleDto = _mapper.Map<ArticleDto>(article);
            return new DataResult<ArticleDto>(ResultStatus.Success, articleDto);

        }
        return new DataResult<ArticleDto>(ResultStatus.Error, message: "An error occurred while getting the article", data: null);
    }

    public async Task<IDataResult<ArticleListDto>> GetAll()
    {
        var articles = await _unitOfWork.Articles.GetAllAsync(null, a => a.User, a => a.Category);
        if (articles.Count > -1)
        {
            //return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            //{
            //    Articles = articles,
            //    ResultStatus = ResultStatus.Success
            //});
            var articleDto = _mapper.Map<ArticleListDto>(articles);
            return new DataResult<ArticleListDto>(ResultStatus.Success, articleDto);
        }
        return new DataResult<ArticleListDto>(ResultStatus.Error, message: "An error occurred while getting the articles", data: null);
    }

    public async Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId)
    {
        var result = await _unitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
        if (result)
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
            if (articles.Count > -1)
            {
                //return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                //{
                //    Articles = articles,
                //    ResultStatus = ResultStatus.Success
                //});
                var articleDto = _mapper.Map<ArticleListDto>(articles);
                return new DataResult<ArticleListDto>(ResultStatus.Success, articleDto);
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, message: "An error occurred while getting the articles", data: null);
        }
        return new DataResult<ArticleListDto>(ResultStatus.Error, message: "An error occurred while getting the categories", data: null);
    }

    public async Task<IDataResult<ArticleListDto>> GetAllByNonDeleted()
    {
        var articles = await _unitOfWork.Articles.GetAllAsync(a => !a.IsDeleted, a => a.User, a => a.Category);
        if (articles.Count > -1)
        {
            //return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            //{
            //    Articles = articles,
            //    ResultStatus = ResultStatus.Success
            //});
            var articleDto = _mapper.Map<ArticleListDto>(articles);
            return new DataResult<ArticleListDto>(ResultStatus.Success, articleDto);
        }
        return new DataResult<ArticleListDto>(ResultStatus.Error, message: "An error occurred while getting the articles", data: null);
    }

    public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActive()
    {
        var articles = await _unitOfWork.Articles.GetAllAsync(a => !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
        if (articles.Count > -1)
        {
            //return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            //{
            //    Articles = articles,
            //    ResultStatus = ResultStatus.Success
            //});
            var articleDto = _mapper.Map<ArticleListDto>(articles);
            return new DataResult<ArticleListDto>(ResultStatus.Success, articleDto);
        }
        return new DataResult<ArticleListDto>(ResultStatus.Error, message: "An error occurred while getting the articles", data: null);
    }

    public async Task<IResult> HardDelete(int articleId)
    {
        var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
        if (result)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
            await _unitOfWork.Articles.DeleteAsync(article);
            await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, $"{article.Title} article has been successfully deleted");
        }
        return new Result(ResultStatus.Error, "An error occurred while deleting the article. Article cannot be found", null);
    }

    public async Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedByName)
    {
        var article = _mapper.Map<Article>(articleUpdateDto);
        article.ModifiedByName = modifiedByName;

        //await _unitOfWork.Articles.UpdateAsync(article).ContinueWith(t => _unitOfWork.SaveAsync());
        await _unitOfWork.Articles.UpdateAsync(article);
        await _unitOfWork.SaveAsync();
        return new Result(ResultStatus.Success, $"{articleUpdateDto.Title} article has been successfully updated");
    }
}
