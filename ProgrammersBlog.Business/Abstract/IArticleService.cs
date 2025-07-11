using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;

namespace ProgrammersBlog.Business.Abstract;

public interface IArticleService
{
    Task<IDataResult<ArticleDto>> GetAsync(int articleId);
    Task<IDataResult<ArticleUpdateDto>> GetArticleUpdateAsync(int articleId);
    Task<IDataResult<ArticleListDto>> GetAllAsync();
    Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync();
    Task<IDataResult<ArticleListDto>> GetAllByDeletedAsync();
    Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize);
    Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync();
    Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId);
    Task<IDataResult<ArticleListDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false);
    Task<IDataResult<ArticleListDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false);
    Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName, int userId);
    Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName);
    Task<IResult> DeleteAsync(int articleId, string modifiedByName);
    Task<IResult> HardDeleteAsync(int articleId);
    Task<IResult> UndoDeleteAsync(int articleId, string modifiedByName);
    Task<IDataResult<int>> CountAsync();
    Task<IDataResult<int>> CountByNonDeletedAsync();

}
