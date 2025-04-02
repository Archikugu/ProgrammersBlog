using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Entities.Dtos.ArticleDtos;

namespace ProgrammersBlog.Business.Abstract;

public interface IArticleService
{
    Task<IDataResult<ArticleDto>> Get(int articleId);
    Task<IDataResult<ArticleListDto>> GetAll();
    Task<IDataResult<ArticleListDto>> GetAllByNonDeleted();
    Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActive();
    Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId);
    Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName);
    Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedByName);
    Task<IResult> Delete(int articleId, string modifiedByName);
    Task<IResult> HardDelete(int articleId);
}
