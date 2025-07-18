﻿using System.Linq.Expressions;
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

public class ArticleManager : ManagerBase, IArticleService
{

    public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {

    }

    public async Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName, int userId)
    {
        var article = Mapper.Map<Article>(articleAddDto);
        article.CreatedByName = createdByName;
        article.ModifiedByName = createdByName;
        article.UserId = userId;
        await UnitOfWork.Articles.AddAsync(article);
        await UnitOfWork.SaveAsync();
        return new Result(ResultStatus.Success, Messages.Article.Add(article.Title));
    }

    public async Task<IResult> DeleteAsync(int articleId, string modifiedByName)
    {
        var result = await UnitOfWork.Articles.AnyAsync(a => a.Id == articleId);
        if (result)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);
            article.IsDeleted = true;
            article.IsActive = false;
            article.ModifiedByName = modifiedByName;
            article.ModifiedDate = DateTime.Now;
            await UnitOfWork.Articles.UpdateAsync(article);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Article.Delete(article.Title));
        }
        return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), null);
    }

    public async Task<IResult> UndoDeleteAsync(int articleId, string modifiedByName)
    {
        var result = await UnitOfWork.Articles.AnyAsync(a => a.Id == articleId);
        if (result)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);
            article.IsDeleted = false;
            article.IsActive = true;
            article.ModifiedByName = modifiedByName;
            article.ModifiedDate = DateTime.Now;
            await UnitOfWork.Articles.UpdateAsync(article);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Article.UndoDelete(article.Title));
        }
        return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), null);
    }

    public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
    {
        var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.User, a => a.Category);

        if (article != null)
        {
            article.Comments = await UnitOfWork.Comments.GetAllAsync(c => c.ArticleId == articleId && !c.IsDeleted && c.IsActive);
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
        var articles = await UnitOfWork.Articles.GetAllAsync(null, a => a.User, a => a.Category);
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
        var result = await UnitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
        if (result)
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
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
        var articles = await UnitOfWork.Articles.GetAllAsync(a => !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
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

    public async Task<IDataResult<ArticleListDto>> GetAllByDeletedAsync()
    {
        var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsDeleted, a => a.User, a => a.Category);
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
        var articles = await UnitOfWork.Articles.GetAllAsync(a => !a.IsDeleted && a.IsActive, a => a.User, a => a.Category);
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
        var result = await UnitOfWork.Articles.AnyAsync(a => a.Id == articleId);
        if (result)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);
            await UnitOfWork.Articles.DeleteAsync(article);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Article.HardDelete(article.Title));
        }
        return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), null);
    }

    public async Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
    {
        var oldArticle = await UnitOfWork.Articles.GetAsync(a => a.Id == articleUpdateDto.Id);
        var article = Mapper.Map<ArticleUpdateDto, Article>(articleUpdateDto, oldArticle);
        article.ModifiedByName = modifiedByName;

        await UnitOfWork.Articles.UpdateAsync(article);
        await UnitOfWork.SaveAsync();
        return new Result(ResultStatus.Success, Messages.Article.Update(article.Title));
    }

    public async Task<IDataResult<int>> CountAsync()
    {
        var articlesCount = await UnitOfWork.Articles.CountAsync();
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
        var articlesCount = await UnitOfWork.Articles.CountAsync(a => !a.IsDeleted);
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
        var result = await UnitOfWork.Articles.AnyAsync(a => a.Id == articleId);
        if (result)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);
            var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(article);
            return new DataResult<ArticleUpdateDto>(ResultStatus.Success, articleUpdateDto);
        }
        else
        {
            return new DataResult<ArticleUpdateDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), null);
        }
    }

    public async Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize)
    {
        var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);

        var sortedArticles = isAscending ? articles.OrderBy(a => a.ViewsCount)
            : articles.OrderByDescending(a => a.ViewsCount);

        return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
        {
            Articles = takeSize == null ? sortedArticles.ToList() : sortedArticles.Take(takeSize.Value).ToList(),
            ResultStatus = ResultStatus.Success
        });
    }

    public async Task<IDataResult<ArticleListDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
    {
        pageSize = pageSize > 20 ? 20 : pageSize;
        var articles = categoryId == null
             ? await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User)
             : await UnitOfWork.Articles.GetAllAsync(a => a.CategoryId == categoryId && a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);

        var sortedArticles = isAscending
            ? articles.OrderBy(a => a.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
            : articles.OrderByDescending(a => a.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

        return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
        {
            Articles = sortedArticles,
            CategoryId = categoryId == null ? null : categoryId.Value,
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = articles.Count,
            IsAscending = isAscending,
            ResultStatus = ResultStatus.Success
        });
    }

    public async Task<IDataResult<ArticleListDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
    {
        pageSize = pageSize > 20 ? 20 : pageSize;
        if (string.IsNullOrWhiteSpace(keyword))
        {
            var articles =
                await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category,
                    a => a.User);
            var sortedArticles = isAscending
                ? articles.OrderBy(a => a.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(a => a.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = sortedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending
            });
        }

        var searchedArticles = await UnitOfWork.Articles.SearchAsync(new List<Expression<Func<Article, bool>>>
            {
                (a) => a.Title.Contains(keyword),
                (a) => a.Category.Name.Contains(keyword),
                (a) => a.SeoDescription.Contains(keyword),
                (a) => a.SeoTags.Contains(keyword)
            },
        a => a.Category, a => a.User);
        var searchedAndSortedArticles = isAscending
            ? searchedArticles.OrderBy(a => a.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
            : searchedArticles.OrderByDescending(a => a.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
        {
            Articles = searchedAndSortedArticles,
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = searchedArticles.Count,
            IsAscending = isAscending
        });
    }
}
