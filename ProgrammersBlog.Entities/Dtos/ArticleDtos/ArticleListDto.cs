using ProgrammersBlog.Core.Entities.Abstract;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.Entities.Dtos.ArticleDtos;

public class ArticleListDto : DtoGetBase
{
    public IList<Article> Articles { get; set; }
}
