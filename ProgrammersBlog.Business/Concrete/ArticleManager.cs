﻿using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Business.Utilities;
using ProgrammersBlog.Core.Entities.Concrete;
using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Core.Utilities.Results.Concrete;
using ProgrammersBlog.DataAccess.Abstract;
using ProgrammersBlog.Entities.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;
using Article = ProgrammersBlog.Entities.Concrete.Article;

namespace ProgrammersBlog.Business.Concrete;

public class ArticleManager : ManagerBase, IArticleService
{
    private readonly UserManager<User> _userManager;

    public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) 
        : base(unitOfWork, mapper)
    {
        _userManager = userManager;
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

        return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false));
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

        return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false));
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

        return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false));
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
        
        return new DataResult<int>(ResultStatus.Error, "An unexpected error occurred", -1);
    }

    public async Task<IDataResult<int>> CountByNonDeletedAsync()
    {
        var articlesCount = await UnitOfWork.Articles.CountAsync(a => !a.IsDeleted);
        if (articlesCount > -1)
        {
            return new DataResult<int>(ResultStatus.Success, articlesCount);
        }
        
        return new DataResult<int>(ResultStatus.Error, "An unexpected error occurred", -1);
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
        
        return new DataResult<ArticleUpdateDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), null);
    }

    public async Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize)
    {
        var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);

        var sortedArticles = isAscending 
            ? articles.OrderBy(a => a.ViewsCount)
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
            var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);
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
        }, a => a.Category, a => a.User);
        
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

    public async Task<IResult> IncreaseViewCountAsync(int articleId)
    {
        var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);
        if (article == null)
        {
            return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false));
        }
        
        article.ViewsCount += 1;
        await UnitOfWork.Articles.UpdateAsync(article);
        await UnitOfWork.SaveAsync();
        
        return new Result(ResultStatus.Success, Messages.Article.IncreaseViewCount(article.Title));
    }

    public async Task<IDataResult<ArticleListDto>> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
    {
        var anyUser = await _userManager.Users.AnyAsync(u => u.Id == userId);
        if (!anyUser)
        {
            return new DataResult<ArticleListDto>(ResultStatus.Error, $"{userId} user not found!.", null);
        }

        var userArticles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted && a.UserId == userId);
        List<Article> sortedArticles = new List<Article>();
        
        switch (filterBy)
        {
            case FilterBy.Category:
                switch (orderBy)
                {
                    case OrderBy.Date:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.Date).ToList()
                            : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                        break;
                    case OrderBy.ViewCount:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.ViewsCount).ToList()
                            : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.ViewsCount).ToList();
                        break;
                    case OrderBy.CommentCount:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.CommentCount).ToList()
                            : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.CommentCount).ToList();
                        break;
                }
                break;
                
            case FilterBy.Date:
                switch (orderBy)
                {
                    case OrderBy.Date:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderBy(a => a.Date).ToList()
                            : userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                        break;
                    case OrderBy.ViewCount:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderBy(a => a.ViewsCount).ToList()
                            : userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderByDescending(a => a.ViewsCount).ToList();
                        break;
                    case OrderBy.CommentCount:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderBy(a => a.CommentCount).ToList()
                            : userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderByDescending(a => a.CommentCount).ToList();
                        break;
                }
                break;
                
            case FilterBy.ViewCount:
                switch (orderBy)
                {
                    case OrderBy.Date:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.ViewsCount >= minViewCount && a.ViewsCount <= maxViewCount).Take(takeSize).OrderBy(a => a.Date).ToList()
                            : userArticles.Where(a => a.ViewsCount >= minViewCount && a.ViewsCount <= maxViewCount).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                        break;
                    case OrderBy.ViewCount:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.ViewsCount >= minViewCount && a.ViewsCount <= maxViewCount).Take(takeSize).OrderBy(a => a.ViewsCount).ToList()
                            : userArticles.Where(a => a.ViewsCount >= minViewCount && a.ViewsCount <= maxViewCount).Take(takeSize).OrderByDescending(a => a.ViewsCount).ToList();
                        break;
                    case OrderBy.CommentCount:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.ViewsCount >= minViewCount && a.ViewsCount <= maxViewCount).Take(takeSize).OrderBy(a => a.CommentCount).ToList()
                            : userArticles.Where(a => a.ViewsCount >= minViewCount && a.ViewsCount <= maxViewCount).Take(takeSize).OrderByDescending(a => a.CommentCount).ToList();
                        break;
                }
                break;
                
            case FilterBy.CommentCount:
                switch (orderBy)
                {
                    case OrderBy.Date:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderBy(a => a.Date).ToList()
                            : userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                        break;
                    case OrderBy.ViewCount:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderBy(a => a.ViewsCount).ToList()
                            : userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderByDescending(a => a.ViewsCount).ToList();
                        break;
                    case OrderBy.CommentCount:
                        sortedArticles = isAscending
                            ? userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderBy(a => a.CommentCount).ToList()
                            : userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderByDescending(a => a.CommentCount).ToList();
                        break;
                }
                break;
        }

        return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
        {
            Articles = sortedArticles
        });
    }

    public async Task<IDataResult<ArticleDto>> GetByIdAsync(int articleId, bool includeCategory, bool includeComments, bool includeUser)
    {
        List<Expression<Func<Article, bool>>> predicates = new List<Expression<Func<Article, bool>>>();
        List<Expression<Func<Article, object>>> includes = new List<Expression<Func<Article, object>>>();
        
        if (includeCategory) includes.Add(a => a.Category);
        if (includeComments) includes.Add(a => a.Comments);
        if (includeUser) includes.Add(a => a.User);
        
        predicates.Add(a => a.Id == articleId);
        var article = await UnitOfWork.Articles.GetAsyncV2(predicates, includes);
        
        if (article == null)
        {
            return new DataResult<ArticleDto>(ResultStatus.Warning, Messages.General.ValidationError(), null,
                new List<ValidationError>
                {
                    new ValidationError
                    {
                        PropertyName = "articleId",
                        Message = Messages.Article.NotFoundById(articleId)
                    }
                });
        }

        return new DataResult<ArticleDto>(ResultStatus.Success, new ArticleDto
        {
            Article = article
        });
    }

    public async Task<IDataResult<ArticleListDto>> GetAllAsyncV2(int? categoryId, int? userId, bool? isActive, bool? isDeleted, int currentPage, int pageSize,
          OrderByGeneral orderBy, bool isAscending, bool includeCategory, bool includeComments, bool includeUser)
    {
        List<Expression<Func<Article, bool>>> predicates = new List<Expression<Func<Article, bool>>>();
        List<Expression<Func<Article, object>>> includes = new List<Expression<Func<Article, object>>>();

        // Predicates
        if (categoryId.HasValue)
        {
            if (!await UnitOfWork.Categories.AnyAsync(c => c.Id == categoryId.Value))
            {
                return new DataResult<ArticleListDto>(ResultStatus.Warning, Messages.General.ValidationError(), null,
                    new List<ValidationError>
                    {
                        new ValidationError
                        {
                            PropertyName = "categoryId",
                            Message = Messages.Category.NotFoundById(categoryId.Value)
                        }
                    });
            }
            predicates.Add(a => a.CategoryId == categoryId.Value);
        }
        
        if (userId.HasValue)
        {
            if (!await _userManager.Users.AnyAsync(u => u.Id == userId.Value))
            {
                return new DataResult<ArticleListDto>(ResultStatus.Warning, Messages.General.ValidationError(), null,
                    new List<ValidationError>
                    {
                        new ValidationError
                        {
                            PropertyName = "userId",
                            Message = Messages.User.NotFoundById(userId.Value)
                        }
                    });
            }
            predicates.Add(a => a.UserId == userId.Value);
        }
        
        if (isActive.HasValue) predicates.Add(a => a.IsActive == isActive.Value);
        if (isDeleted.HasValue) predicates.Add(a => a.IsDeleted == isDeleted.Value);
        
        // Includes
        if (includeCategory) includes.Add(a => a.Category);
        if (includeComments) includes.Add(a => a.Comments);
        if (includeUser) includes.Add(a => a.User);
        
        var articles = await UnitOfWork.Articles.GetAllAsyncV2(predicates, includes);

        IOrderedEnumerable<Article> sortedArticles;

        switch (orderBy)
        {
            case OrderByGeneral.Id:
                sortedArticles = isAscending ? articles.OrderBy(a => a.Id) : articles.OrderByDescending(a => a.Id);
                break;
            case OrderByGeneral.Az:
                sortedArticles = isAscending ? articles.OrderBy(a => a.Title) : articles.OrderByDescending(a => a.Title);
                break;
            // Default CreatedDate
            default:
                sortedArticles = isAscending ? articles.OrderBy(a => a.CreatedDate) : articles.OrderByDescending(a => a.CreatedDate);
                break;
        }

        return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
        {
            Articles = sortedArticles.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList(),
            CategoryId = categoryId.HasValue ? categoryId.Value : null,
            CurrentPage = currentPage,
            PageSize = pageSize,
            IsAscending = isAscending,
            TotalCount = articles.Count,
            ResultStatus = ResultStatus.Success
        });
    }
}
