using ProgrammersBlog.Entities.Dtos.ArticleDtos;

namespace ProgrammersBlog.MvcUI.Models;

public class ArticleDetailViewModel
{
    public ArticleDto ArticleDto { get; set; }
    public ArticleDetailRightSideBarViewModel ArticleDetailRightSideBarViewModel { get; set; }
}
