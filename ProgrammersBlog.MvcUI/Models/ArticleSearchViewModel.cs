using ProgrammersBlog.Entities.Dtos.ArticleDtos;

namespace ProgrammersBlog.MvcUI.Models
{
    public class ArticleSearchViewModel
    {
        public ArticleListDto ArticleListDto { get; set; }
        public string Keyword { get; set; }
    }
}
    